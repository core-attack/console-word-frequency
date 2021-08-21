using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoremNET;

namespace console_word_frequency
{
    public class TxtFileGenerator : IFileGenerator
    {
        public TxtFileGenerator()
        {
        }

        public async Task GenerateFilesAsync(string path, int filesCount, int levels, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < filesCount; i++)
            {
                var p = PathCombine(path, GetSubPath((int)Lorem.Number(0, levels)));
                tasks.Add(GenerateFileAsync(p, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        public async Task GenerateFileAsync(string path, CancellationToken cancellationToken)
        {
            var fileName = $"{DateTime.UtcNow.Ticks}.txt";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllLinesAsync(Path.Combine(path, fileName), Lorem.Paragraphs(10, 1000, 1, 100, 1, 10), cancellationToken);
        }

        private string GetSubPath(int level)
        {
            if (level == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for(var i = 0; i < level; i++)
            {
                sb.AppendFormat(@"/{0}", i);
            }

            return sb.ToString();
        }

        private string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }
    }
}
