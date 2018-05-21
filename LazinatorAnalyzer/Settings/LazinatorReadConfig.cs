using System;
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
        private const string ConfigFileName = "LazinatorConfig.json";
        private const string AltConfigFileName = "Lazinatorconfig.json";

        public static (string path, string text) GetConfigPathAndText(ImmutableArray<AdditionalText> additionalFiles,
            CancellationToken cancellationToken)
        {
            var file = additionalFiles.SingleOrDefault(f =>
                string.Compare(Path.GetFileName(f.Path), ConfigFileName, StringComparison.OrdinalIgnoreCase) == 0);
            if (file == null)
            {
                file = additionalFiles.SingleOrDefault(f =>
                   string.Compare(Path.GetFileName(f.Path), AltConfigFileName, StringComparison.OrdinalIgnoreCase) == 0);
                if (file == null)
                    return (null, null);
            }

            var fileText = file.GetText();

            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    fileText.Write(writer, cancellationToken);
                }

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return (Path.GetDirectoryName(file.Path), reader.ReadToEnd());
                }
            }
        }
    }
}
