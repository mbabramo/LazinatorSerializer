using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace LazinatorAnalyzer.Analyzer
{


    public static class FixAllContextHelper
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
                        documentDiagnostics = documentDiagnostics.Distinct().ToImmutableArray();
                        var dictionary = ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Empty.SetItem(document, documentDiagnostics);
                        return dictionary;
                    }

                    break;

                case FixAllScope.Project:
                    projectsToFix = ImmutableArray.Create(project);
                    allDiagnostics = await GetAllDiagnosticsInProjectAsync(fixAllContext, project).ConfigureAwait(false);
                    break;

                case FixAllScope.Solution:
                    projectsToFix = project.Solution.Projects
                        .Where(p => p.Language == project.Language)
                        .ToImmutableArray();

                    var diagnosticsByProject = new ConcurrentDictionary<ProjectId, ImmutableArray<Diagnostic>>();
                    var tasks = new Task[projectsToFix.Length];
                    for (int i = 0; i < projectsToFix.Length; i++)
                    {
                        fixAllContext.CancellationToken.ThrowIfCancellationRequested();
                        var projectToFix = projectsToFix[i];
                        tasks[i] = Task.Run(
                            async () =>
                            {
                                var projectDiagnostics = await GetAllDiagnosticsInProjectAsync(fixAllContext, projectToFix).ConfigureAwait(false);
                                diagnosticsByProject.TryAdd(projectToFix.Id, projectDiagnostics);
                            }, fixAllContext.CancellationToken);
                    }

                    await Task.WhenAll(tasks).ConfigureAwait(false);
                    allDiagnostics = allDiagnostics.AddRange(diagnosticsByProject.SelectMany(i => i.Value.Where(x => fixAllContext.DiagnosticIds.Contains(x.Id))));
                    break;
            }

            if (allDiagnostics.IsEmpty)
            {
                return ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Empty;
            }

            return await BuildDocumentDiagnosticsDictionary(allDiagnostics, projectsToFix, fixAllContext.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all <see cref="Diagnostic"/> instances within a specific <see cref="Project"/> which are relevant to a
        /// <see cref="FixAllContext"/>.
        /// </summary>
        /// <param name="fixAllContext">The context for the Fix All operation.</param>
        /// <param name="project">The project.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. When the task completes
        /// successfully, the <see cref="Task{TResult}.Result"/> will contain the requested diagnostics.</returns>
        private static async Task<ImmutableArray<Diagnostic>> GetAllDiagnosticsInProjectAsync(FixAllContext fixAllContext, Project project)
        {
            var result = await fixAllContext.GetAllDiagnosticsAsync(project).ConfigureAwait(false);
            // This may return hundreds of redundant diagnostics. Perhaps that indicates that there is something wrong either with GetAllDiagnosticsAsync or with code in our project. This may be slowing down compilation. But in the meantime, we can at least save time by eliminating redundancies.
            return result.Distinct().ToImmutableArray();
        }

        private static async Task<ImmutableDictionary<Document, ImmutableArray<Diagnostic>>> BuildDocumentDiagnosticsDictionary(
            ImmutableArray<Diagnostic> diagnostics,
            ImmutableArray<Project> projects,
            CancellationToken cancellationToken)
        {
            ImmutableDictionary<SyntaxTree, Document> treeToDocumentMap = await GetTreeToDocumentMapAsync(projects, cancellationToken).ConfigureAwait(false);

            var builder = ImmutableDictionary.CreateBuilder<Document, ImmutableArray<Diagnostic>>();
            foreach (var documentAndDiagnostics in diagnostics.GroupBy(d => GetReportedDocument(d, treeToDocumentMap)))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var document = documentAndDiagnostics.Key;
                var diagnosticsForDocument = documentAndDiagnostics.Distinct().ToImmutableArray();
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
