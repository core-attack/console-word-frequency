using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace console_word_frequency
{
    class Program
    {
        static async Task Main()
        {
            var defaultPath = "files";
            var defaultOutputFile = "output";
            var repeatKeys = new[] { 'y', 'Y', 'н', 'Н' };

            var close = true;

            IFileGenerator generator = new TxtFileGenerator();
            IWordCounter counter = new TxtWordCounter();
            IWordSorter sorter = new WordSorter();
            var cancellationToken = new CancellationToken();

            do
            {
                var path = ReadParam($"Enter the files directory (or '{defaultPath}' will be used): ", defaultPath);
                var outputFileName = ReadParam($"Enter the output file name (or '{defaultOutputFile}' will be used): ", defaultOutputFile);

                //await generator.GenerateFilesAsync(path, 100, 10, cancellationToken);
                var unsorted = await counter.CountWords(path, outputFileName, cancellationToken);
                var sorted = await sorter.Sort(unsorted);

                await WriteResult(sorted, cancellationToken);

                Console.Write($"One more time? [y/n]");

                var key = Console.ReadKey();

                close = Array.Exists(repeatKeys, c => key.Equals(c));
            }
            while (close);
        }

        private static async Task WriteResult(WordCounterResult stats, CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(stats.OutputFile, stats.GetPrettyStatistic(), cancellationToken);
        }

        private static string ReadParam(string message, string defaultValue)
        {
            Console.Write(message);
            var param = Console.ReadLine();
            return string.IsNullOrEmpty(param) ? defaultValue : param;
        }
    }
}
