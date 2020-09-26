using System;
using System.IO;

namespace LazinatorTests.Support
{
    public static class ReadCodeFile
    {

        public static string GetNameOfType(Type existingType)
        {
            string name = existingType.Name;
            for (int genericIndex = 1; genericIndex < 5; genericIndex++)
            if (name.Contains($"`{genericIndex}"))
                name = name.Replace($"`{genericIndex}", "");

            return name;
        }

        public static void GetCodeInFile(string projectPath, string mainFolder, string subfolder, string fileNameWithoutExtension, string extension, out string fullPath, out string code)
        {
            fullPath = projectPath + mainFolder + subfolder + fileNameWithoutExtension + extension;
            if (File.Exists(fullPath))
            {
                using (TextReader textReader = File.OpenText(fullPath))
                    code = textReader.ReadToEnd();
            }
            else
                code = null;

        }

        public static string GetCodeBasePath(string project = "")
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string codeBaseLocation = assembly.Location;
            string projectInOverallFolder = "LazinatorSerializer\\" ;
            string throughProject =
                codeBaseLocation.Substring(0, codeBaseLocation.IndexOf(projectInOverallFolder) + projectInOverallFolder.Length) + project;
            //throughProject = throughProject.Substring("file:///".Length);
            return throughProject;
        }
    }
}
