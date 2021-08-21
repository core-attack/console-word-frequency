using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace console_word_frequency
{
    public class Program
    {
        private static string DefaultPath = "files";
        private static string DefaultOutputFile = "output";

        static async Task Main()
        {
            var summary = BenchmarkRunner.Run<Benchmarking>();
            
            var repeatKeys = new[] { 'y', 'Y', 'н', 'Н' };

            var close = true;

            IFileGenerator generator = new TxtFileGenerator();
            IWordCounter counter = new TxtWordCounter();
            IWordSorter sorter = new WordSorter();
            var cancellationToken = new CancellationToken();

            do
            {
                var path = ReadParam($"Enter the files directory (or '{DefaultPath}' will be used): ", DefaultPath);
                var outputFileName = ReadParam($"Enter the output file name (or '{DefaultOutputFile}' will be used): ", DefaultOutputFile);

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
