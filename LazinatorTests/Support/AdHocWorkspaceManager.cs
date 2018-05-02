using LazinatorCodeGen.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Support
{
    public static class AdhocWorkspaceManager
    {
        public static async Task<Compilation> GetCompilation(AdhocWorkspace ws)
        {
            var project = ws.CurrentSolution.Projects.First();
            var compilation = await project.GetCompilationAsync();
            return compilation;
        }

        public static AdhocWorkspace CreateAdHocWorkspaceWithAllFilesInProjects(List<string> projects)
        {
            List<(string filename, string code)> filesWithCode = new List<(string filename, string code)>();
            foreach (string project in projects)
            {
                var fullPathNames = Directory.EnumerateFiles(project, "*.*", SearchOption.AllDirectories).ToList();
                foreach (string fullPathName in fullPathNames)
                    if (fullPathName.EndsWith(".cs"))
                    {
                        string code = null;
                        using (TextReader textReader = File.OpenText(fullPathName))
                            code = textReader.ReadToEnd();
                        filesWithCode.Add((fullPathName, code));
                    }
            }
            return CreateAdHocWorkspaceWithFiles(filesWithCode.Where(x => x.code != null).ToList());
        }

        public static AdhocWorkspace CreateAdHocWorkspaceWithFiles(List<(string project, string mainFolder, string subfolder, string filename)> fileinfos)
        {
            List<(string filename, string code)> files = new List<(string filename, string code)>();
            foreach (var (project, mainFolder, subfolder, filename) in fileinfos)
            {
                string projectPath = ReadCodeFile.GetCodeBasePath(project);
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, filename, ".cs", out string mainCodePath, out string mainCode);
                if (mainCode == null)
                    ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "", filename, ".cs", out mainCodePath, out mainCode);
                files.Add((filename + ".cs", mainCode));
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, filename, ".g.cs", out string codeBehindPath, out string codeBehind);
                if (codeBehind == null)
                    ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "", filename, ".g.cs", out codeBehindPath, out codeBehind);
                files.Add((filename + ".g.cs", codeBehind));
            }
            return CreateAdHocWorkspaceWithFiles(files.Where(x => x.code != null).ToList());
        }

        public static AdhocWorkspace CreateAdHocWorkspaceWithFiles(List<(string filename, string code)> files)
        {
            var adhocWorkspace = new AdhocWorkspace();
            ProjectInfo project = ProjectInfo.Create(ProjectId.CreateNewId("FakeProject"), VersionStamp.Create(DateTime.Now), "FakeProject", "FakeProject.dll", LanguageNames.CSharp);

            project = project.WithDocuments(files.Select(file => DocumentInfo.Create(DocumentId.CreateNewId(project.Id, file.filename), file.filename, loader: TextLoader.From(TextAndVersion.Create(SourceText.From(file.code), VersionStamp.Create())))));
            project = AddProjectReferences(project);

            adhocWorkspace.AddProject(project);

            return adhocWorkspace;
        }

        public static AdhocWorkspace CreateAdHocWorkspaceFromSource(string source)
        {
            var adhocWorkspace = new AdhocWorkspace();
            ProjectInfo project = ProjectInfo.Create(ProjectId.CreateNewId("FakeProject"), VersionStamp.Create(DateTime.Now), "FakeProject", "FakeProject.dll", LanguageNames.CSharp);
            project = AddProjectReferences(project);

            DocumentInfo document = DocumentInfo.Create(DocumentId.CreateNewId(project.Id, "source.cs"), "source.cs", loader: TextLoader.From(TextAndVersion.Create(SourceText.From(source), VersionStamp.Create())));
            project = project.WithDocuments(new DocumentInfo[] { document });

            adhocWorkspace.AddProject(project);

            return adhocWorkspace;
        }

        private static ProjectInfo AddProjectReferences(ProjectInfo project)
        {
            var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            //var neededAssemblies = new[]
            //{
            //    "Build",
            //    "System",
            //    "mscorlib",
            //    "Lazinator"
            //};
            List<PortableExecutableReference> references = trustedAssembliesPaths
                // Apparently, we need some other ones. .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();
            project = project.WithMetadataReferences(references);
            return project;
        }
    }
}
