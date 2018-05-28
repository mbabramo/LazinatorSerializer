using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazinatorAnalyzer.Settings
{
    public static class LazinatorConfigLoader
    {
        private const string ConfigFileName = "LazinatorConfig.json";
        private const string AltConfigFileName = "Lazinatorconfig.json";

        // The following two methods are essentially the same, but one works with files of type AdditionalText (available from the analyzer) and one works with files of type TextDocument (available directly from the project). Repeating the code is simpler than creating a common interface type and converting.

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

        public async static Task<(string path, string text)> GetConfigPathAndText(ImmutableArray<TextDocument> additionalFiles,
            CancellationToken cancellationToken)
        {
            var file = additionalFiles.SingleOrDefault(f =>
                string.Compare(Path.GetFileName(f.FilePath), ConfigFileName, StringComparison.OrdinalIgnoreCase) == 0);
            if (file == null)
            {
                file = additionalFiles.SingleOrDefault(f =>
                    string.Compare(Path.GetFileName(f.FilePath), AltConfigFileName, StringComparison.OrdinalIgnoreCase) == 0);
                if (file == null)
                    return (null, null);
            }

            var fileText = await file.GetTextAsync();

            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    fileText.Write(writer, cancellationToken);
                }

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return (Path.GetDirectoryName(file.FilePath), reader.ReadToEnd());
                }
            }
        }
    }
}
