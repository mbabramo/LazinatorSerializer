
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Text;


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
            if (fixAllContext.Document != null)
            {
                var documentsAndDiagnosticsToFixMap = await this.GetDocumentDiagnosticsToFixAsync(fixAllContext).ConfigureAwait(false);
                return await this.GetFixAsync(documentsAndDiagnosticsToFixMap, fixAllContext).ConfigureAwait(false);
            }
            else
            {
                var projectsAndDiagnosticsToFixMap = await this.GetProjectDiagnosticsToFixAsync(fixAllContext).ConfigureAwait(false);
                return await this.GetFixAsync(projectsAndDiagnosticsToFixMap, fixAllContext).ConfigureAwait(false);
            }
        }

        public virtual async Task<CodeAction> GetFixAsync(
            ImmutableDictionary<Document, ImmutableArray<Diagnostic>> documentsAndDiagnosticsToFixMap,
            FixAllContext fixAllContext)
        {
            if (documentsAndDiagnosticsToFixMap != null && documentsAndDiagnosticsToFixMap.Any())
            {
                fixAllContext.CancellationToken.ThrowIfCancellationRequested();

                var documents = documentsAndDiagnosticsToFixMap.Keys.ToImmutableArray();
                var fixesBag = new List<CodeAction>[documents.Length];
                var fixOperations = new List<Task>(documents.Length);
                for (int index = 0; index < documents.Length; index++)
                {
                    if (fixAllContext.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var document = documents[index];
                    fixesBag[index] = new List<CodeAction>();
                    fixOperations.Add(this.AddDocumentFixesAsync(document, documentsAndDiagnosticsToFixMap[document], fixesBag[index].Add, fixAllContext));
                }

                await Task.WhenAll(fixOperations).ConfigureAwait(false);

                if (fixesBag.Any(fixes => fixes.Count > 0))
                {
                    return await this.TryGetMergedFixAsync(fixesBag.SelectMany(i => i), fixAllContext).ConfigureAwait(false);
                }
            }

            return null;
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
                            // TODO: Can we share code between similar lambdas that we pass to this API in BatchFixAllProvider.cs, CodeFixService.cs and CodeRefactoringService.cs?
                            // Serialize access for thread safety - we don't know what thread the fix provider will call this delegate from.
                            lock (localFixes)
                            {
                                localFixes.Add(a);
                            }
                        },
                        cancellationToken);

                    // TODO: Wrap call to ComputeFixesAsync() below in IExtensionManager.PerformFunctionAsync() so that
                    // a buggy extension that throws can't bring down the host?
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

        public virtual async Task<CodeAction> GetFixAsync(
            ImmutableDictionary<Project, ImmutableArray<Diagnostic>> projectsAndDiagnosticsToFixMap,
            FixAllContext fixAllContext)
        {
            if (projectsAndDiagnosticsToFixMap != null && projectsAndDiagnosticsToFixMap.Any())
            {
                var fixesBag = new List<CodeAction>[projectsAndDiagnosticsToFixMap.Count];
                var fixOperations = new List<Task>(projectsAndDiagnosticsToFixMap.Count);
                int index = -1;
                foreach (var project in projectsAndDiagnosticsToFixMap.Keys)
                {
                    if (fixAllContext.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    index++;
                    var diagnostics = projectsAndDiagnosticsToFixMap[project];
                    fixesBag[index] = new List<CodeAction>();
                    fixOperations.Add(this.AddProjectFixesAsync(project, diagnostics, fixesBag[index].Add, fixAllContext));
                }

                await Task.WhenAll(fixOperations).ConfigureAwait(false);

                if (fixesBag.Any(fixes => fixes.Count > 0))
                {
                    return await this.TryGetMergedFixAsync(fixesBag.SelectMany(i => i), fixAllContext).ConfigureAwait(false);
                }
            }

            return null;
        }

        public virtual Task AddProjectFixesAsync(Project project, IEnumerable<Diagnostic> diagnostics, Action<CodeAction> addFix, FixAllContext fixAllContext)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<CodeAction> TryGetMergedFixAsync(IEnumerable<CodeAction> batchOfFixes, FixAllContext fixAllContext)
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
            {
                var title = this.GetFixAllTitle(fixAllContext);
                return CodeAction.Create(title, cancellationToken => Task.FromResult(newSolution));
            }

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

        public virtual Task<ImmutableDictionary<Document, ImmutableArray<Diagnostic>>> GetDocumentDiagnosticsToFixAsync(FixAllContext fixAllContext)
        {
            return FixAllContextHelper.GetDocumentDiagnosticsToFixAsync(fixAllContext);
        }

        public virtual Task<ImmutableDictionary<Project, ImmutableArray<Diagnostic>>> GetProjectDiagnosticsToFixAsync(FixAllContext fixAllContext)
        {
            return FixAllContextHelper.GetProjectDiagnosticsToFixAsync(fixAllContext);
        }

        public virtual async Task<Solution> TryMergeFixesAsync(Solution oldSolution, IEnumerable<CodeAction> codeActions, CancellationToken cancellationToken)
        {
            var changedDocumentsMap = new Dictionary<DocumentId, Document>();
            Dictionary<DocumentId, List<Document>> documentsToMergeMap = null;

            var currentSolution = oldSolution;

            foreach (var codeAction in codeActions)
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
                    continue;
                }

                var changedSolution = singleApplyChangesOperation.ChangedSolution;
                var solutionChanges = changedSolution.GetChanges(oldSolution);

                // TODO: Handle removed documents
                // TODO: Handle changed/added/removed additional documents


                var documentIdsForNew = solutionChanges
                    .GetProjectChanges()
                    .SelectMany(p => p.GetAddedDocuments());

                foreach (var documentId in documentIdsForNew)
                {
                    Document document1 = changedSolution.GetDocument(documentId);
                    var documentText = await document1.GetTextAsync(cancellationToken).ConfigureAwait(false);
                    if (!currentSolution.ContainsDocument(documentId))
                        currentSolution = currentSolution.AddDocument(DocumentInfo.Create(documentId, document1.Name, document1.Folders));
                    currentSolution = currentSolution.WithDocumentText(documentId, documentText);
                }

                var documentIdsWithChanges = solutionChanges
                    .GetProjectChanges()
                    .SelectMany(p => p.GetChangedDocuments());


                foreach (var documentId in documentIdsWithChanges)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var document = changedSolution.GetDocument(documentId);

                    Document existingDocument;
                    if (changedDocumentsMap.TryGetValue(documentId, out existingDocument))
                    {
                        if (existingDocument != null)
                        {
                            changedDocumentsMap[documentId] = null;
                            var documentsToMerge = new List<Document>();
                            documentsToMerge.Add(existingDocument);
                            documentsToMerge.Add(document);
                            documentsToMergeMap = documentsToMergeMap ?? new Dictionary<DocumentId, List<Document>>();
                            documentsToMergeMap[documentId] = documentsToMerge;
                        }
                        else
                        {
                            documentsToMergeMap[documentId].Add(document);
                        }
                    }
                    else
                    {
                        changedDocumentsMap[documentId] = document;
                    }
                }
            }

            foreach (var kvp in changedDocumentsMap)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var document = kvp.Value;
                if (document != null)
                {
                    var documentText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
                    currentSolution = currentSolution.WithDocumentText(kvp.Key, documentText);
                }
            }

            if (documentsToMergeMap != null)
            {
                var mergedDocuments = new ConcurrentDictionary<DocumentId, SourceText>();
                var documentsToMergeArray = documentsToMergeMap.ToImmutableArray();
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

    internal static class FixAllContextHelper
    {
        public static async Task<ImmutableDictionary<Document, ImmutableArray<Diagnostic>>> GetDocumentDiagnosticsToFixAsync(FixAllContext fixAllContext)
        {
            var allDiagnostics = ImmutableArray<Diagnostic>.Empty;
            var projectsToFix = ImmutableArray<Project>.Empty;

            var document = fixAllContext.Document;
            var project = fixAllContext.Project;

            switch (fixAllContext.Scope)
            {
                case FixAllScope.Document:
                    if (document != null)
                    {
                        var documentDiagnostics = await fixAllContext.GetDocumentDiagnosticsAsync(document).ConfigureAwait(false);
                        return ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Empty.SetItem(document, documentDiagnostics);
                    }

                    break;

                case FixAllScope.Project:
                    projectsToFix = ImmutableArray.Create(project);
                    allDiagnostics = await GetAllDiagnosticsAsync(fixAllContext, project).ConfigureAwait(false);
                    break;

                case FixAllScope.Solution:
                    projectsToFix = project.Solution.Projects
                        .Where(p => p.Language == project.Language)
                        .ToImmutableArray();

                    var diagnostics = new ConcurrentDictionary<ProjectId, ImmutableArray<Diagnostic>>();
                    var tasks = new Task[projectsToFix.Length];
                    for (int i = 0; i < projectsToFix.Length; i++)
                    {
                        fixAllContext.CancellationToken.ThrowIfCancellationRequested();
                        var projectToFix = projectsToFix[i];
                        tasks[i] = Task.Run(
                            async () =>
                            {
                                var projectDiagnostics = await GetAllDiagnosticsAsync(fixAllContext, projectToFix).ConfigureAwait(false);
                                diagnostics.TryAdd(projectToFix.Id, projectDiagnostics);
                            }, fixAllContext.CancellationToken);
                    }

                    await Task.WhenAll(tasks).ConfigureAwait(false);
                    allDiagnostics = allDiagnostics.AddRange(diagnostics.SelectMany(i => i.Value.Where(x => fixAllContext.DiagnosticIds.Contains(x.Id))));
                    break;
            }

            if (allDiagnostics.IsEmpty)
            {
                return ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Empty;
            }

            return await GetDocumentDiagnosticsToFixAsync(allDiagnostics, projectsToFix, fixAllContext.CancellationToken).ConfigureAwait(false);
        }

        public static async Task<ImmutableDictionary<Project, ImmutableArray<Diagnostic>>> GetProjectDiagnosticsToFixAsync(FixAllContext fixAllContext)
        {
            var project = fixAllContext.Project;
            if (project != null)
            {
                switch (fixAllContext.Scope)
                {
                    case FixAllScope.Project:
                        var diagnostics = await fixAllContext.GetProjectDiagnosticsAsync(project).ConfigureAwait(false);
                        return ImmutableDictionary<Project, ImmutableArray<Diagnostic>>.Empty.SetItem(project, diagnostics);

                    case FixAllScope.Solution:
                        var projectsAndDiagnostics = new ConcurrentDictionary<Project, ImmutableArray<Diagnostic>>();
                        var tasks = new List<Task>(project.Solution.ProjectIds.Count);
                        Func<Project, Task> projectAction =
                            async proj =>
                            {
                                if (fixAllContext.CancellationToken.IsCancellationRequested)
                                {
                                    return;
                                }

                                var projectDiagnostics = await fixAllContext.GetProjectDiagnosticsAsync(proj).ConfigureAwait(false);
                                if (projectDiagnostics.Any())
                                {
                                    projectsAndDiagnostics.TryAdd(proj, projectDiagnostics);
                                }
                            };

                        foreach (var proj in project.Solution.Projects)
                        {
                            if (fixAllContext.CancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            tasks.Add(projectAction(proj));
                        }

                        await Task.WhenAll(tasks).ConfigureAwait(false);
                        return projectsAndDiagnostics.ToImmutableDictionary();
                }
            }

            return ImmutableDictionary<Project, ImmutableArray<Diagnostic>>.Empty;
        }

        /// <summary>
        /// Gets all <see cref="Diagnostic"/> instances within a specific <see cref="Project"/> which are relevant to a
        /// <see cref="FixAllContext"/>.
        /// </summary>
        /// <param name="fixAllContext">The context for the Fix All operation.</param>
        /// <param name="project">The project.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. When the task completes
        /// successfully, the <see cref="Task{TResult}.Result"/> will contain the requested diagnostics.</returns>
        private static async Task<ImmutableArray<Diagnostic>> GetAllDiagnosticsAsync(FixAllContext fixAllContext, Project project)
        {
            var result = await fixAllContext.GetAllDiagnosticsAsync(project).ConfigureAwait(false);
            // This may return hundreds of redundant diagnostics. Perhaps that indicates that there is something wrong either with GetAllDiagnosticsAsync or with code in our project. This is likely slowing down compilation. But in the meantime, we can at least save time by eliminating redundancies.
            return result.Distinct().ToImmutableArray();
        }

        private static async Task<ImmutableDictionary<Document, ImmutableArray<Diagnostic>>> GetDocumentDiagnosticsToFixAsync(
            ImmutableArray<Diagnostic> diagnostics,
            ImmutableArray<Project> projects,
            CancellationToken cancellationToken)
        {
            var treeToDocumentMap = await GetTreeToDocumentMapAsync(projects, cancellationToken).ConfigureAwait(false);

            var builder = ImmutableDictionary.CreateBuilder<Document, ImmutableArray<Diagnostic>>();
            foreach (var documentAndDiagnostics in diagnostics.GroupBy(d => GetReportedDocument(d, treeToDocumentMap)))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var document = documentAndDiagnostics.Key;
                var diagnosticsForDocument = documentAndDiagnostics.ToImmutableArray();
                builder.Add(document, diagnosticsForDocument);
            }

            return builder.ToImmutable();
        }

        private static async Task<ImmutableDictionary<SyntaxTree, Document>> GetTreeToDocumentMapAsync(ImmutableArray<Project> projects, CancellationToken cancellationToken)
        {
            var builder = ImmutableDictionary.CreateBuilder<SyntaxTree, Document>();
            foreach (var project in projects)
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var document in project.Documents)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                    builder.Add(tree, document);
                }
            }

            return builder.ToImmutable();
        }

        private static Document GetReportedDocument(Diagnostic diagnostic, ImmutableDictionary<SyntaxTree, Document> treeToDocumentsMap)
        {
            var tree = diagnostic.Location.SourceTree;
            if (tree != null)
            {
                Document document;
                if (treeToDocumentsMap.TryGetValue(tree, out document))
                {
                    return document;
                }
            }

            return null;
        }
    }
}