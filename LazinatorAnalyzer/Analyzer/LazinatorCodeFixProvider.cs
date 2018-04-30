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

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(LazinatorCodeAnalyzer.Lazin001); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            SourceFileInformation sourceFileInfo = new SourceFileInformation(
                await context.Document.GetSemanticModelAsync(), diagnostic.Properties, diagnostic.AdditionalLocations);

            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            if (diagnostic.Id == LazinatorCodeAnalyzer.Lazin001)
            {
                var syntaxToken = root.FindToken(diagnosticSpan.Start);
                var declaration = syntaxToken.Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Lazin001Title,
                        createChangedSolution: c => FixGenerateLazinatorCodeBehind(context.Document, declaration, sourceFileInfo, c),
                        equivalenceKey: Lazin001Title),
                    diagnostic);
            }
        }
        
        private async Task<Solution> FixGenerateLazinatorCodeBehind(Document originalDocument, TypeDeclarationSyntax enclosingType, SourceFileInformation sourceFileInformation, CancellationToken cancellationToken)
        {
            if (!(enclosingType is ClassDeclarationSyntax) && !(enclosingType is StructDeclarationSyntax))
                throw new Exception("Could not fix. Attribute is not in a class or struct.");
            var originalSolution = originalDocument.Project.Solution;

            try
            {
                LazinatorConfig config = null;
                if (sourceFileInformation.Config != null)
                {
                    try
                    {
                        config = new LazinatorConfig(sourceFileInformation.Config);
                    }
                    catch
                    {
                        throw new LazinatorCodeGenException("Lazinator.config is not a valid JSON file.");
                    }
                }

                var semanticModel = await originalDocument.GetSemanticModelAsync(cancellationToken);
                LazinatorCompilation generator = new LazinatorCompilation(semanticModel.Compilation, sourceFileInformation.LazinatorObject.Name, sourceFileInformation.LazinatorObject.GetFullNamespace() + "." + sourceFileInformation.LazinatorObject.MetadataName, config);
                var d = new ObjectDescription(generator.ImplementingTypeSymbol, generator);
                var codeBehind = d.GetCodeBehind();

                if (sourceFileInformation.CodeBehindLocation == null)
                {
                    var project = originalDocument.Project;
                    string codeBehindName = originalDocument.Name.Substring(0, originalDocument.Name.Length - 3) + ".g.cs";
                    string codeBehindFilePath = originalDocument.FilePath.Substring(0, originalDocument.FilePath.Length - 3) + ".g.cs"; // replace .cs with .g.cs
                    Document documentToAdd = project.AddDocument(codeBehindName, codeBehind, null, codeBehindFilePath).WithFolders(originalDocument.Folders);
                    var revisedSolution = documentToAdd.Project.Solution;
                    return revisedSolution;
                }
                else
                {
                    var currentDocumentWithCode = originalSolution.GetDocument(sourceFileInformation.CodeBehindLocation.SourceTree);
                    var replacementDocument = currentDocumentWithCode.WithText(SourceText.From(codeBehind));
                    var revisedSolution = originalSolution.WithDocumentText(currentDocumentWithCode.Id, SourceText.From(codeBehind));
                    return revisedSolution;
                }
            }
            catch (LazinatorCodeGenException e)
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

                return replacementDocument.Project.Solution;
            }

        }
    }
}
