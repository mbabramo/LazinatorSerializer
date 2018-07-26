using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lazinator.CodeDescription;
using LazinatorAnalyzer.Settings;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace LazinatorAnalyzer.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LazinatorCodeFixProvider)), Shared]
    public class LazinatorCodeFixProvider : CodeFixProvider
    {
        private const string Lazin001Title = "Generate Lazinator code behind";
        private const string Lazin002Title = "Regenerate Lazinator code behind";
        private const string Lazin004Title = "Remove Lazinator code behind";

        public static bool RecycleLazinatorCompilation { get; set; }
        public static LazinatorCompilation _LastLazinatorCompilation { get; set; }

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(LazinatorCodeAnalyzer.Lazin001, LazinatorCodeAnalyzer.Lazin002, LazinatorCodeAnalyzer.Lazin004); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return new LazinatorCustomFixAllProvider();
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics
                .Where(x => FixableDiagnosticIds.Contains(x.Id))
                .FirstOrDefault();
            if (diagnostic == null)
                return;
            LazinatorPairInformation lazinatorPairInfo = new LazinatorPairInformation(
                await context.Document.GetSemanticModelAsync(), diagnostic.Properties, diagnostic.AdditionalLocations);

            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            if (diagnostic.Id == LazinatorCodeAnalyzer.Lazin001 || diagnostic.Id == LazinatorCodeAnalyzer.Lazin002)
            {
                var syntaxToken = root.FindToken(diagnosticSpan.Start);
                var declaration = syntaxToken.Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

                // Register a code action that will invoke the fix.
                string title = diagnostic.Id == LazinatorCodeAnalyzer.Lazin001 ? Lazin001Title : Lazin002Title;
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: title,
                        createChangedSolution: c => FixGenerateLazinatorCodeBehind(context.Document, declaration, lazinatorPairInfo, c),
                        equivalenceKey: title),
                    diagnostic);
            }

            if (diagnostic.Id == LazinatorCodeAnalyzer.Lazin004)
            {
                string title = Lazin004Title;
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: title,
                        createChangedSolution: c => DeleteExtraFiles(context.Document.Project.Solution,  lazinatorPairInfo.IncorrectCodeBehindLocations, c),
                        equivalenceKey: title),
                    diagnostic);
            }
        }

        public static Task<Solution> DeleteExtraFiles(Solution originalSolution, List<Location> extraFiles, CancellationToken cancellationToken)
        {
            Solution revisedSolution = originalSolution;
            if (extraFiles != null)
                foreach (var d in extraFiles.Select(x => originalSolution.GetDocument(x.SourceTree)))
                    revisedSolution = originalSolution.RemoveDocument(d.Id);
            return Task.FromResult(revisedSolution);
        }

        public static async Task<Solution> FixGenerateLazinatorCodeBehind(Document originalDocument, TypeDeclarationSyntax enclosingType, LazinatorPairInformation lazinatorPairInformation, CancellationToken cancellationToken)
        {

            if (!(enclosingType is ClassDeclarationSyntax) && !(enclosingType is StructDeclarationSyntax))
                throw new LazinatorCodeGenException("Could not fix. Attribute is not in a class or struct.");

            try
            {
                if (originalDocument == null)
                    return null;
                Solution revisedSolution = await AttemptFixGenerateLazinatorCodeBehind(originalDocument, lazinatorPairInformation, cancellationToken);
                return revisedSolution;
            }
            catch (LazinatorCodeGenException e)
            {
                Solution revisedSolution = await InsertLazinatorCodeGenerationError(originalDocument, enclosingType, e, cancellationToken);
                return revisedSolution;
            }
        }

        public static async Task<Solution> AttemptFixGenerateLazinatorCodeBehind(Document originalDocument, LazinatorPairInformation lazinatorPairInformation, CancellationToken cancellationToken)
        {
            var originalSolution = originalDocument.Project.Solution;
            LazinatorConfig config = lazinatorPairInformation.LoadLazinatorConfig();

            var semanticModel = await originalDocument.GetSemanticModelAsync(cancellationToken);
            LazinatorCompilation generator = null;
            if (!RecycleLazinatorCompilation || _LastLazinatorCompilation == null)
            {
                generator = new LazinatorCompilation(semanticModel.Compilation, lazinatorPairInformation.LazinatorObject.Name, lazinatorPairInformation.LazinatorObject.GetFullMetadataName(), config);
                if (RecycleLazinatorCompilation)
                    _LastLazinatorCompilation = generator;
            }
            else
            {
                generator = _LastLazinatorCompilation;
                generator.Initialize(semanticModel.Compilation, lazinatorPairInformation.LazinatorObject.Name, lazinatorPairInformation.LazinatorObject.GetFullMetadataName());
            }
            var d = new ObjectDescription(generator.ImplementingTypeSymbol, generator);
            var codeBehind = d.GetCodeBehind();

            string fileExtension = config?.GeneratedCodeFileExtension ?? ".laz.cs";
            var project = originalDocument.Project;
            string codeBehindFilePath = null;
            string codeBehindName = null;
            string[] codeBehindFolders = null;
            bool useFullyQualifiedNames = (config?.UseFullyQualifiedNames ?? false) || generator.ImplementingTypeSymbol.ContainingType != null || generator.ImplementingTypeSymbol.IsGenericType;
            codeBehindName = RoslynHelpers.GetEncodableVersionOfIdentifier(generator.ImplementingTypeSymbol, useFullyQualifiedNames) + fileExtension;
            if (config?.GeneratedCodePath == null)
            { // use short form of name in same location as original code

                codeBehindFilePath = originalDocument.FilePath;
            }
            else
            { // we have a config file specifying a common directory
                codeBehindFilePath = config.GeneratedCodePath;
                if (!codeBehindFilePath.EndsWith("\\"))
                    codeBehindFilePath += "\\";
                codeBehindFolders = config.RelativeGeneratedCodePath.Split('\\', '/');
            }
            codeBehindFilePath = System.IO.Path.GetDirectoryName(codeBehindFilePath);
            while (codeBehindFilePath.EndsWith(".cs"))
            {
                var lastSlash = codeBehindFilePath.IndexOfAny(new char[] { '\\', '/' });
                if (lastSlash >= 0)
                    codeBehindFilePath = codeBehindFilePath.Substring(0, lastSlash);
            }
            while (codeBehindFilePath.EndsWith("\\"))
                codeBehindFilePath = codeBehindFilePath.Substring(0, codeBehindFilePath.Length - 1);
            codeBehindFilePath += "\\" + codeBehindName;

            Solution revisedSolution;
            if (lazinatorPairInformation.CodeBehindLocation == null)
            { // the file does not already exist 
              // codeBehindFilePath = System.IO.Path.GetDirectoryName(codeBehindFilePath); // omit file name
                Document documentToAdd = project.AddDocument(codeBehindName, codeBehind, codeBehindFolders, codeBehindFilePath);
                //if (config.GeneratedCodePath != null)
                //    documentToAdd = documentToAdd.WithFolders(codeBehindFolders);
                revisedSolution = documentToAdd.Project.Solution;
            }
            else
            { // the file does already exist
                var currentDocumentWithCode = originalSolution.GetDocument(lazinatorPairInformation.CodeBehindLocation.SourceTree);
                var replacementDocument = currentDocumentWithCode.WithText(SourceText.From(codeBehind));
                revisedSolution = originalSolution.WithDocumentText(currentDocumentWithCode.Id, SourceText.From(codeBehind));
            }
            revisedSolution = await AddAnnotationToIndicateSuccess(revisedSolution, true);
            return revisedSolution;
        }

        private static async Task<Solution> InsertLazinatorCodeGenerationError(Document originalDocument, TypeDeclarationSyntax enclosingType, LazinatorCodeGenException e, CancellationToken cancellationToken)
        {
            // We need a mechanism for reporting a problem. Simply throwing an exception will lead to a message indicating that Lazinator is not working,
            // so that is not a good option. 
            // One approach that we tried was to add a Syntax annotation. The theory was that if we note the existence of this annotation, we won't throw the regular
            // error, and instead can put in a different error. The problem is that Roslyn did not re-analyze the document, and rebuilding would cause the annotation 
            // to disappear, it seemed. 
            // So, instead, we'll just insert a comment before the type declaration. 
            var enclosingTypeWithComment =
                enclosingType
                    .WithLeadingTrivia(
                        enclosingType.GetLeadingTrivia()
                            .Add(SyntaxFactory.Comment($"/* Lazinator ERROR: {e.Message} */ ")));
            var originalRoot = await originalDocument.GetSyntaxRootAsync(cancellationToken);
            var replacementRoot = originalRoot.ReplaceNode(enclosingType, enclosingTypeWithComment);
            var replacementDocument = originalDocument.WithSyntaxRoot(replacementRoot);

            var revisedSolution = replacementDocument.Project.Solution;
            revisedSolution = await AddAnnotationToIndicateSuccess(revisedSolution, false);
            return revisedSolution;
        }

        private static async Task<Solution> AddAnnotationToIndicateSuccess(Solution revisedSolution, bool successStatus)
        {
            var documentInSolution = revisedSolution.Projects.FirstOrDefault()?.Documents?.FirstOrDefault();
            var syntaxRoot = await documentInSolution.GetSyntaxRootAsync();
            var revisedRoot = syntaxRoot
                .WithoutAnnotations(new SyntaxAnnotation("Lazinator"))
                .WithAdditionalAnnotations(new SyntaxAnnotation("Lazinator", successStatus ? "Success" : "Failure"));
            revisedSolution = revisedSolution.WithDocumentSyntaxRoot(documentInSolution.Id, revisedRoot);
            return revisedSolution;
        }

        public static async Task<bool> WhetherCodeFixFailed(Solution revisedSolution)
        {
            var documentInSolution = revisedSolution.Projects.FirstOrDefault()?.Documents?.FirstOrDefault();
            var syntaxRoot = await documentInSolution.GetSyntaxRootAsync();
            bool hasAnnotations = syntaxRoot.HasAnnotations("Lazinator");
            var annotations = syntaxRoot.GetAnnotations("Lazinator");
            if (annotations.Any())
            {
                bool failure = annotations.First().Data == "Failure";
                return failure;
            }
            return false;
        }
    }
}
