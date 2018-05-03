﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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
            string codeBaseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string projectInOverallFolder = "LazinatorSerializer/" + project;
            string throughProject =
                codeBaseLocation.Substring(0, codeBaseLocation.IndexOf(projectInOverallFolder) + projectInOverallFolder.Length);
            string removingPrefix = throughProject.Substring("file:///".Length);
            return removingPrefix;
        }
    }
}
