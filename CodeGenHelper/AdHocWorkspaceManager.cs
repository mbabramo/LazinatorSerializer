using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenHelper
{
    public static class AdhocWorkspaceManager
    {
        public static async Task<Compilation> GetCompilation(AdhocWorkspace ws)
        {
            var project = ws.CurrentSolution.Projects.First();
            var compilation = await project.GetCompilationAsync();
            return compilation!;
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
                        string code = "";
                        using (TextReader textReader = File.OpenText(fullPathName))
                            code = textReader.ReadToEnd();
                        filesWithCode.Add((fullPathName, code));
                    }
            }
            return CreateAdHocWorkspaceWithFiles(filesWithCode.Where(x => x.code != null).ToList());
        }

        public static AdhocWorkspace CreateAdHocWorkspaceWithFiles(List<(string project, string mainFolder, string subfolder, string filename)> fileinfos, string generatedExtension)
        {
            List<(string filename, string code)> files = new List<(string filename, string code)>();
            foreach (var (project, mainFolder, subfolder, filename) in fileinfos)
            {
                string projectPath = ReadCodeFile.GetCodeBasePath(project);
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, filename, ".cs", out string mainCodePath, out string mainCode);
                if (mainCode == null)
                    ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "", filename, ".cs", out mainCodePath, out mainCode);
                files.Add((filename + ".cs", mainCode));
                ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, filename, generatedExtension, out string codeBehindPath, out string codeBehind);
                if (codeBehind == null)
                    ReadCodeFile.GetCodeInFile(projectPath, mainFolder, "", filename, generatedExtension, out codeBehindPath, out codeBehind);
                files.Add((filename + generatedExtension, codeBehind));
            }
            return CreateAdHocWorkspaceWithFiles(files.Where(x => x.code != null).ToList());
        }

        public static AdhocWorkspace CreateAdHocWorkspaceWithFiles(List<(string filename, string code)> files)
        {
            var adhocWorkspace = new AdhocWorkspace();
            ProjectInfo project = ProjectInfo.Create(ProjectId.CreateNewId("FakeProject"), VersionStamp.Create(DateTime.Now.ToUniversalTime()), "FakeProject", "FakeProject.dll", LanguageNames.CSharp);

            project = project.WithDocuments(files.Select(file => DocumentInfo.Create(DocumentId.CreateNewId(project.Id, file.filename), file.filename, loader: TextLoader.From(TextAndVersion.Create(SourceText.From(file.code), VersionStamp.Create())))));
            project = AddProjectReferences(project);

            adhocWorkspace.AddProject(project);

            return adhocWorkspace;
        }

        public static AdhocWorkspace CreateAdHocWorkspaceFromSource(List<string> sources)
        {
            var adhocWorkspace = new AdhocWorkspace();
            ProjectInfo project = ProjectInfo.Create(ProjectId.CreateNewId("FakeProject"), VersionStamp.Create(DateTime.Now), "FakeProject", "FakeProject.dll", LanguageNames.CSharp);
            project = AddProjectReferences(project);

            DocumentInfo[] documents = sources.Select(source => DocumentInfo.Create(DocumentId.CreateNewId(project.Id, "source.cs"), "source.cs", loader: TextLoader.From(TextAndVersion.Create(SourceText.From(source), VersionStamp.Create())))).ToArray();
            project = project.WithDocuments(documents);

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
            List<PortableExecutableReference> references = GetProjectReferences();
            project = project.WithMetadataReferences(references);
            return project;
        }

        public static List<PortableExecutableReference> GetProjectReferences()
        {
            var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
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
            return references;
        }
    }
}
