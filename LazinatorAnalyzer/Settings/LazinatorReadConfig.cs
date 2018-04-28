﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazinatorAnalyzer.Settings
{
    public static class ConfigLoader
    {
        private const string ConfigFileName = "LazinatorConfig.config";

        private static string LoadConfig(ImmutableArray<AdditionalText> additionalFiles,
            CancellationToken cancellationToken)
        {
            var file = additionalFiles.SingleOrDefault(f =>
                string.Compare(Path.GetFileName(f.Path), ConfigFileName, StringComparison.OrdinalIgnoreCase) == 0);
            if (file == null)
            {
                return null;
            }

            var fileText = file.GetText(cancellationToken);

            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    fileText.Write(writer, cancellationToken);
                }

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            var config = LoadConfig(context.Options.AdditionalFiles, context.CancellationToken);
        }
    }
}
