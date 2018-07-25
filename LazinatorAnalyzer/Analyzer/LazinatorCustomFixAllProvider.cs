
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lazinator.CodeDescription;
using LazinatorAnalyzer.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;


namespace LazinatorAnalyzer.Analyzer
{
    /// <summary>
    /// Helper class for "Fix all occurrences" code fix providers.
    /// </summary>
    public partial class LazinatorCustomFixAllProvider : FixAllProvider
    {
        public LazinatorCustomFixAllProvider()
        {
        }

        public static FixAllProvider Instance { get; } = new LazinatorCustomFixAllProvider();

        public override async Task<CodeAction> GetFixAsync(FixAllContext fixAllContext)
        {
            try
            {
                LazinatorCodeFixProvider.RecycleLazinatorCompilation = true;
                LazinatorCodeFixProvider._LastLazinatorCompilation = null;
                // We allow for the possibility that our initial solution will still need work. 
                const int MaxStages = 5;
                Solution revisedSolution = await GenerateRevisedSolution(fixAllContext, MaxStages);
                CodeAction result = await this.GetCodeActionToFixAll(revisedSolution, fixAllContext).ConfigureAwait(false);
                return result;
            }
            finally
            {
                LazinatorCodeFixProvider.RecycleLazinatorCompilation = false;
            }
        }

        private async Task<Solution> GenerateRevisedSolution(FixAllContext fixAllContext, int maxStagesAfterThisOne = 0)
        {
            var documentsAndDiagnosticsToFixMap = await GetDocumentDiagnosticsToFixAsync(fixAllContext).ConfigureAwait(false);
            Solution revisedSolution;
            if (documentsAndDiagnosticsToFixMap.Any())
            {
                var solutionAfterStage = await GenerateSolutionFromProblemsList(documentsAndDiagnosticsToFixMap, fixAllContext);
                if (maxStagesAfterThisOne == 0)
                    revisedSolution = solutionAfterStage;
                else
                    revisedSolution = await GenerateRevisedSolution(GetNextStageFixAllContext(fixAllContext, solutionAfterStage), maxStagesAfterThisOne - 1);
            }
            else
                revisedSolution = fixAllContext.Solution;
            return revisedSolution;
        }

        private FixAllContext GetNextStageFixAllContext(FixAllContext originalContext, Solution revisedSolution)
        {
            FixAllContext.DiagnosticProvider diagnosticProvider = GetDiagnosticProvider(originalContext);
            if (diagnosticProvider == null)
                return null; // Roslyn changed -- won't be able to do multi-stage work
            FixAllContext revisedContext;
            if (originalContext.Document != null)
            {
                var id = originalContext.Document.Id;
                var documentInRevised = revisedSolution.GetDocument(id);
                revisedContext = new FixAllContext(documentInRevised, originalContext.CodeFixProvider, originalContext.Scope, originalContext.CodeActionEquivalenceKey, originalContext.DiagnosticIds, diagnosticProvider, originalContext.CancellationToken);
            }
            else
            {
                var id = originalContext.Project.Id;
                var projectInRevised = revisedSolution.GetProject(id);
                revisedContext = new FixAllContext(projectInRevised, originalContext.CodeFixProvider, originalContext.Scope, originalContext.CodeActionEquivalenceKey, originalContext.DiagnosticIds, diagnosticProvider, originalContext.CancellationToken);
            }
            return revisedContext;
        }

        private FixAllContext.DiagnosticProvider GetDiagnosticProvider(FixAllContext context)
        {
            // This is a hack to get the DiagnosticProvider. 
            try
            {
                object fixAllState = GetNonpublicProperty(context, "State"); // returns an internal type FixAllState
                FixAllContext.DiagnosticProvider diagnosticProvider = (FixAllContext.DiagnosticProvider)GetNonpublicProperty(fixAllState, "DiagnosticProvider");
                return diagnosticProvider;
            }
            catch
            {
                return null;
            }
        }

        private U GetNonpublicProperty<T, U>(T containingInstance, string propertyName)
        {
            return (U) GetNonpublicProperty<T>(containingInstance, propertyName);
        }

        private object GetNonpublicProperty<T>(T containingInstance, string propertyName)
        {
            return GetNonpublicProperty(containingInstance, propertyName);
        }

        private static object GetNonpublicProperty(object containingInstance, string propertyName)
        {
            Type containingType = containingInstance.GetType();
            PropertyInfo prop = containingType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo getter = prop.GetGetMethod(nonPublic: true);
            object bar = getter.Invoke(containingInstance, null);
            return bar;
        }

        public async Task<ImmutableDictionary<Document, ImmutableArray<Diagnostic>>> GetDocumentDiagnosticsToFixAsync(
            FixAllContext fixAllContext)
        {
            var document = fixAllContext.Document;
            var project = fixAllContext.Project;
            var builder = ImmutableDictionary.CreateBuilder<Document, ImmutableArray<Diagnostic>>();

            switch (fixAllContext.Scope)
            {
                case FixAllScope.Document:
                    await AddDiagnosticsToBuilder(document, builder, fixAllContext.CancellationToken);
                    break;

                case FixAllScope.Project:
                    await AddDiagnosticsToBuilder(project, builder, fixAllContext.CancellationToken);
                    break;

                case FixAllScope.Solution:
                    await AddDiagnosticsToBuilder(project.Solution, builder, fixAllContext.CancellationToken);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return builder.ToImmutable();
        }

        public async Task AddDiagnosticsToBuilder(Solution s, ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Builder builder, CancellationToken cancellationToken)
        {
            if (s != null)
                foreach (var project in s.Projects)
                    await AddDiagnosticsToBuilder(project, builder, cancellationToken);
        }

        public async Task AddDiagnosticsToBuilder(Project p, ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Builder builder, CancellationToken cancellationToken)
        {
            if (p == null)
                return;
            Compilation compilation = await p.GetCompilationAsync();
            var additionalDocuments = p.AdditionalDocuments;
            LazinatorCompilationAnalyzer analyzer = await
                LazinatorCompilationAnalyzer.CreateCompilationAnalyzer(compilation, cancellationToken, additionalDocuments?.ToImmutableArray() ?? new ImmutableArray<TextDocument>());
            if (analyzer == null)
                return;
            analyzer.DisableStartingFromInterface = true;
            var config = analyzer.Config;
            foreach (var doc in p.Documents.Where(x => x.SourceCodeKind == SourceCodeKind.Regular && !x.FilePath.EndsWith(config?.GeneratedCodeFileExtension ?? ".laz.cs")))
            {
                await AddDiagnosticsForDocument(builder, compilation, additionalDocuments, analyzer, doc, cancellationToken);
            }
        }

        public async Task AddDiagnosticsToBuilder(Document d, ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Builder builder, CancellationToken cancellationToken)
        {
            if (d == null)
                return;
            Project p = d.Project;
            Compilation compilation = await p.GetCompilationAsync();
            var additionalDocuments = p.AdditionalDocuments;
            LazinatorCompilationAnalyzer analyzer = await
                LazinatorCompilationAnalyzer.CreateCompilationAnalyzer(compilation, cancellationToken, additionalDocuments.ToImmutableArray());
            await AddDiagnosticsForDocument(builder, compilation, additionalDocuments, analyzer, d, cancellationToken);
        }

        private static async Task AddDiagnosticsForDocument(ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Builder builder, Compilation compilation, IEnumerable<TextDocument> additionalDocuments, LazinatorCompilationAnalyzer analyzer, Document doc, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (analyzer == null)
            {
                analyzer = await
                    LazinatorCompilationAnalyzer.CreateCompilationAnalyzer(compilation, cancellationToken, additionalDocuments.ToImmutableArray());
                analyzer.DisableStartingFromInterface = true;
            }
            SyntaxNode root = await doc.GetSyntaxRootAsync(cancellationToken);
            var model = compilation.GetSemanticModel(root.SyntaxTree);
            var namedTypes = root.DescendantNodesAndSelf()
                .OfType<TypeDeclarationSyntax>()
                .Select(x => model.GetDeclaredSymbol(x))
                .OfType<INamedTypeSymbol>()
                .ToList();
            foreach (var symbol in namedTypes)
                analyzer.AnalyzeNamedType(symbol, compilation);
            ImmutableArray<Diagnostic> diagnostics = analyzer.GetDiagnosticsToReport().ToImmutableArray();
            if (diagnostics.Any())
                builder.Add(doc, diagnostics);
            analyzer.ClearDiagnostics();
        }

        public virtual Task<CodeAction> GetCodeActionToFixAll(
            Solution revisedSolution,
            FixAllContext fixAllContext)
        {
            var title = this.GetFixAllTitle(fixAllContext);
            CodeAction result = CodeAction.Create(title, cancellationToken => Task.FromResult(revisedSolution));
            return Task.FromResult(result);
        }

        public virtual async Task<Solution> GenerateSolutionFromProblemsList(ImmutableDictionary<Document, ImmutableArray<Diagnostic>> documentsAndDiagnosticsToFixMap,
            FixAllContext fixAllContext)
        {
            Solution revisedSolution = null;
            if (documentsAndDiagnosticsToFixMap != null && documentsAndDiagnosticsToFixMap.Any())
            {
                fixAllContext.CancellationToken.ThrowIfCancellationRequested();

                var documents = documentsAndDiagnosticsToFixMap.Keys.ToImmutableArray();

                // calculate fixes
                var codeActionsForEachDocument = new List<CodeAction>[documents.Length];
                var tasksToGenerateCodeActionsForEachDocument = new List<Task>(documents.Length);
                for (int index = 0; index < documents.Length; index++)
                {
                    if (fixAllContext.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var document = documents[index];
                    codeActionsForEachDocument[index] = new List<CodeAction>();
                    tasksToGenerateCodeActionsForEachDocument.Add(this.AddDocumentFixesAsync(document, documentsAndDiagnosticsToFixMap[document], codeActionsForEachDocument[index].Add, fixAllContext));
                }

                await Task.WhenAll(tasksToGenerateCodeActionsForEachDocument).ConfigureAwait(false);

                if (codeActionsForEachDocument.Any(fixes => fixes.Count > 0))
                {
                    revisedSolution = await this.TryGetMergedFixAsync(codeActionsForEachDocument.SelectMany(i => i), fixAllContext).ConfigureAwait(false);
                }
            }
            return revisedSolution;
        }

        public async virtual Task AddDocumentFixesAsync(Document document, ImmutableArray<Diagnostic> diagnostics, Action<CodeAction> addFix, FixAllContext fixAllContext)
        {
            Debug.Assert(!diagnostics.IsDefault, "!diagnostics.IsDefault");
            var cancellationToken = fixAllContext.CancellationToken;
            var fixerTasks = new Task[diagnostics.Length];
            var fixes = new List<CodeAction>[diagnostics.Length];

            for (var i = 0; i < diagnostics.Length; i++)
            {
                int currentFixIndex = i;
                cancellationToken.ThrowIfCancellationRequested();
                var diagnostic = diagnostics[i];
                fixerTasks[i] = Task.Run(async () =>
                {
                    var localFixes = new List<CodeAction>();
                    var context = new CodeFixContext(
                        document,
                        diagnostic,
                        (a, d) =>
                        {
                            // Serialize access for thread safety - we don't know what thread the fix provider will call this delegate from.
                            lock (localFixes)
                            {
                                localFixes.Add(a);
                            }
                        },
                        cancellationToken);

                    // TODO: Wrap call to ComputeFixesAsync() below in IExtensionManager.PerformFunctionAsync() so that a buggy extension that throws can't bring down the host?
                    var task = fixAllContext.CodeFixProvider.RegisterCodeFixesAsync(context) ?? Task.CompletedTask;
                    await task.ConfigureAwait(false);

                    cancellationToken.ThrowIfCancellationRequested();
                    localFixes.RemoveAll(action => action.EquivalenceKey != fixAllContext.CodeActionEquivalenceKey);
                    fixes[currentFixIndex] = localFixes;
                });
            }

            await Task.WhenAll(fixerTasks).ConfigureAwait(false);
            foreach (List<CodeAction> fix in fixes)
            {
                if (fix == null)
                {
                    continue;
                }

                foreach (CodeAction action in fix)
                {
                    addFix(action);
                }
            }
        }

        public virtual async Task<Solution> TryGetMergedFixAsync(IEnumerable<CodeAction> batchOfFixes, FixAllContext fixAllContext)
        {
            if (batchOfFixes == null)
            {
                throw new ArgumentNullException(nameof(batchOfFixes));
            }

            if (!batchOfFixes.Any())
            {
                throw new ArgumentException($"{nameof(batchOfFixes)} cannot be empty.", nameof(batchOfFixes));
            }

            var solution = fixAllContext.Solution;
            var newSolution = await this.TryMergeFixesAsync(solution, batchOfFixes, fixAllContext.CancellationToken).ConfigureAwait(false);
            if (newSolution != null && newSolution != solution)
                return newSolution; 

            return null;
        }

        public virtual string GetFixAllTitle(FixAllContext fixAllContext)
        {
            var diagnosticIds = fixAllContext.DiagnosticIds;
            string diagnosticId;
            if (diagnosticIds.Count == 1)
            {
                diagnosticId = diagnosticIds.Single();
            }
            else
            {
                diagnosticId = string.Join(",", diagnosticIds.ToArray());
            }

            switch (fixAllContext.Scope)
            {
                case FixAllScope.Custom:
                    return string.Format("Fix all '{0}'", diagnosticId);

                case FixAllScope.Document:
                    var document = fixAllContext.Document;
                    return string.Format("Fix all '{0}' in '{1}'", diagnosticId, document.Name);

                case FixAllScope.Project:
                    var project = fixAllContext.Project;
                    return string.Format("Fix all '{0}' in '{1}'", diagnosticId, project.Name);

                case FixAllScope.Solution:
                    return string.Format("Fix all '{0}' in Solution", diagnosticId);

                default:
                    throw new InvalidOperationException("Not reachable");
            }
        }

        public class RevisionsTracker
        {
            public HashSet<DocumentId> removedDocuments = new HashSet<DocumentId>();
            public Dictionary<DocumentId, Document> newDocumentsMap = new Dictionary<DocumentId, Document>();
            public Dictionary<DocumentId, Document> changedDocumentsMap = new Dictionary<DocumentId, Document>();
            public Dictionary<DocumentId, List<Document>> documentsToMergeMap = null;
        }

        public virtual async Task<Solution> TryMergeFixesAsync(Solution oldSolution, IEnumerable<CodeAction> codeActions, CancellationToken cancellationToken)
        {
            Solution currentSolution = oldSolution;
            RevisionsTracker revisionsTracker;
            List<CodeAction> codeActionsToProcess = codeActions.ToList();
            revisionsTracker = new RevisionsTracker();
            foreach (var codeAction in codeActionsToProcess)
            {
                bool success = await IncludeCodeActionInRevisionsAsync(currentSolution, codeAction, revisionsTracker, cancellationToken);
            }
            currentSolution = await UpdateSolutionFromRevisionsAsync(currentSolution, revisionsTracker, cancellationToken);

            return currentSolution;
        }

        public async Task<bool> IncludeCodeActionInRevisionsAsync(Solution oldSolution, CodeAction codeAction, RevisionsTracker revisionsTracker, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO: Parallelize GetChangedSolutionInternalAsync for codeActions
            ImmutableArray<CodeActionOperation> operations = await codeAction.GetPreviewOperationsAsync(cancellationToken).ConfigureAwait(false);
            ApplyChangesOperation singleApplyChangesOperation = null;
            foreach (var operation in operations)
            {
                if (!(operation is ApplyChangesOperation applyChangesOperation))
                {
                    continue;
                }

                if (singleApplyChangesOperation != null)
                {
                    // Already had an ApplyChangesOperation; only one is supported.
                    singleApplyChangesOperation = null;
                    break;
                }

                singleApplyChangesOperation = applyChangesOperation;
            }

            if (singleApplyChangesOperation == null)
            {
                return false;
            }

            var changedSolution = singleApplyChangesOperation.ChangedSolution;
            
            if (await LazinatorCodeFixProvider.WhetherCodeFixFailed(changedSolution))
                return false;

            var solutionChanges = changedSolution.GetChanges(oldSolution);
            
            foreach (var projectChange in solutionChanges.GetProjectChanges())
                foreach (var removed in projectChange.GetRemovedDocuments())
                    revisionsTracker.removedDocuments.Add(removed);

            var documentIdsForNew = solutionChanges
                .GetProjectChanges()
                .SelectMany(p => p.GetAddedDocuments());

            foreach (var documentId in documentIdsForNew)
            {
                if (revisionsTracker.newDocumentsMap.ContainsKey(documentId))
                    throw new LazinatorCodeGenException("Internal exception. Did not expect multiple code fixes to produce same new document.");
                revisionsTracker.newDocumentsMap[documentId] = changedSolution.GetDocument(documentId);
            }

            var documentIdsWithChanges = solutionChanges
                .GetProjectChanges()
                .SelectMany(p => p.GetChangedDocuments());

            foreach (var documentId in documentIdsWithChanges)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var document = changedSolution.GetDocument(documentId);

                Document existingDocument;
                if (revisionsTracker.changedDocumentsMap.TryGetValue(documentId, out existingDocument))
                {
                    if (existingDocument != null)
                    {
                        revisionsTracker.changedDocumentsMap[documentId] = null;
                        var documentsToMerge = new List<Document>();
                        documentsToMerge.Add(existingDocument);
                        documentsToMerge.Add(document);
                        revisionsTracker.documentsToMergeMap = revisionsTracker.documentsToMergeMap ?? new Dictionary<DocumentId, List<Document>>();
                        revisionsTracker.documentsToMergeMap[documentId] = documentsToMerge;
                    }
                    else
                    {
                        revisionsTracker.documentsToMergeMap[documentId].Add(document);
                    }
                }
                else
                {
                    revisionsTracker.changedDocumentsMap[documentId] = document;
                }
            }

            return true;

        }

        private static async Task<Solution> UpdateSolutionFromRevisionsAsync(Solution oldSolution, RevisionsTracker revisionsTracker, CancellationToken cancellationToken)
        {
            var currentSolution = oldSolution;

            foreach (DocumentId removedDoc in revisionsTracker.removedDocuments)
                currentSolution = currentSolution.RemoveDocument(removedDoc);

            foreach (KeyValuePair<DocumentId, Document> doc in revisionsTracker.newDocumentsMap)
            {
                var documentText = await doc.Value.GetTextAsync(cancellationToken).ConfigureAwait(false);
                if (!currentSolution.ContainsDocument(doc.Key))
                    currentSolution = currentSolution.AddDocument(DocumentInfo.Create(doc.Key, doc.Value.Name, doc.Value.Folders));
                currentSolution = currentSolution.WithDocumentText(doc.Key, documentText);
            }

            foreach (var kvp in revisionsTracker.changedDocumentsMap)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var document = kvp.Value;
                if (document != null)
                {
                    var documentText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
                    currentSolution = currentSolution.WithDocumentText(kvp.Key, documentText);
                }
            }

            if (revisionsTracker.documentsToMergeMap != null)
            {
                var mergedDocuments = new ConcurrentDictionary<DocumentId, SourceText>();
                var documentsToMergeArray = revisionsTracker.documentsToMergeMap.ToImmutableArray();
                var mergeTasks = new Task[documentsToMergeArray.Length];
                for (int i = 0; i < documentsToMergeArray.Length; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var kvp = documentsToMergeArray[i];
                    var documentId = kvp.Key;
                    var documentsToMerge = kvp.Value;
                    var oldDocument = oldSolution.GetDocument(documentId);

                    mergeTasks[i] = Task.Run(async () =>
                    {
                        var appliedChanges = (await documentsToMerge[0].GetTextChangesAsync(oldDocument, cancellationToken).ConfigureAwait(false)).ToList();

                        foreach (var document in documentsToMerge.Skip(1))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            appliedChanges = await TryAddDocumentMergeChangesAsync(
                                oldDocument,
                                document,
                                appliedChanges,
                                cancellationToken).ConfigureAwait(false);
                        }

                        var oldText = await oldDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
                        var newText = oldText.WithChanges(appliedChanges);
                        mergedDocuments.TryAdd(documentId, newText);
                    });
                }

                await Task.WhenAll(mergeTasks).ConfigureAwait(false);

                foreach (var kvp in mergedDocuments)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    currentSolution = currentSolution.WithDocumentText(kvp.Key, kvp.Value);
                }
            }

            return currentSolution;
        }

        /// <summary>
        /// Try to merge the changes between <paramref name="newDocument"/> and <paramref name="oldDocument"/> into <paramref name="cumulativeChanges"/>.
        /// If there is any conflicting change in <paramref name="newDocument"/> with existing <paramref name="cumulativeChanges"/>, then the original <paramref name="cumulativeChanges"/> are returned.
        /// Otherwise, the newly merged changes are returned.
        /// </summary>
        /// <param name="oldDocument">Base document on which FixAll was invoked.</param>
        /// <param name="newDocument">New document with a code fix that is being merged.</param>
        /// <param name="cumulativeChanges">Existing merged changes from other batch fixes into which newDocument changes are being merged.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<List<TextChange>> TryAddDocumentMergeChangesAsync(
            Document oldDocument,
            Document newDocument,
            List<TextChange> cumulativeChanges,
            CancellationToken cancellationToken)
        {
            var successfullyMergedChanges = new List<TextChange>();

            int cumulativeChangeIndex = 0;
            foreach (var change in await newDocument.GetTextChangesAsync(oldDocument, cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                while (cumulativeChangeIndex < cumulativeChanges.Count && cumulativeChanges[cumulativeChangeIndex].Span.End < change.Span.Start)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Existing change that does not overlap with the current change in consideration
                    successfullyMergedChanges.Add(cumulativeChanges[cumulativeChangeIndex]);
                    cumulativeChangeIndex++;
                }

                if (cumulativeChangeIndex < cumulativeChanges.Count)
                {
                    var cumulativeChange = cumulativeChanges[cumulativeChangeIndex];
                    if (!cumulativeChange.Span.IntersectsWith(change.Span))
                    {
                        // The current change in consideration does not intersect with any existing change
                        successfullyMergedChanges.Add(change);
                    }
                    else
                    {
                        if (change.Span != cumulativeChange.Span || change.NewText != cumulativeChange.NewText)
                        {
                            // The current change in consideration overlaps an existing change but
                            // the changes are not identical.
                            // Bail out merge efforts and return the original 'cumulativeChanges'.
                            return cumulativeChanges;
                        }
                        else
                        {
                            // The current change in consideration is identical to an existing change
                            successfullyMergedChanges.Add(change);
                            cumulativeChangeIndex++;
                        }
                    }
                }
                else
                {
                    // The current change in consideration does not intersect with any existing change
                    successfullyMergedChanges.Add(change);
                }
            }

            while (cumulativeChangeIndex < cumulativeChanges.Count)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Existing change that does not overlap with the current change in consideration
                successfullyMergedChanges.Add(cumulativeChanges[cumulativeChangeIndex]);
                cumulativeChangeIndex++;
            }

            return successfullyMergedChanges;
        }
    }
}