using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task GenerateFilesAsync(string path, int filesCount, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            for (int i = 1; i <= filesCount; i++)
            {
                tasks.Add(GenerateFileAsync(path, cancellationToken));
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
    }
}
