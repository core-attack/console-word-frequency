using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoremNET;

namespace ConsoleWordFrequency.Generators
{
    public abstract class FileGenerator
    {
        public virtual string GetFileName(string fileName) => $"{fileName}.{Extension}";

        protected string Extension { get; set; }

        protected Encoding Encoding { get; set; }

        public virtual async Task GenerateFilesAsync(string path, int filesCount, int levels, int wordsCount, int sentencesCount, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < filesCount; i++)
            {
                var p = PathCombine(path, GetSubPath((int)Lorem.Number(0, levels)));
                var content = Lorem.Paragraph(wordsCount, sentencesCount);
                tasks.Add(GenerateFileAsync(p, content, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        public async Task GenerateFileAsync(string path, string content, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllTextAsync(Path.Combine(path, GetFileName(DateTime.UtcNow.Ticks.ToString())), content, Encoding, cancellationToken);
        }

        public void GenerateFile(string path, string filename, string content)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(Path.Combine(path, GetFileName(filename)), content, Encoding);
        }


        private string GetSubPath(int level)
        {
            if (level == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < level; i++)
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
