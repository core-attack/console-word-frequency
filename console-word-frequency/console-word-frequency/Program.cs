using System;
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

            var generator = new TxtFileGenerator();

            do
            {
                Console.Write($"Enter the files directory (or '{defaultPath}' will be used): ");

                var path = Console.ReadLine();

                if (string.IsNullOrEmpty(path))
                {
                    path = defaultPath;
                }

                Console.Write($"Enter the output file name (or '{defaultOutputFile}' will be used): ");

                var outputFileName = Console.ReadLine();

                if (string.IsNullOrEmpty(outputFileName))
                {
                    outputFileName = defaultOutputFile;
                }

                //await generator.GenerateFilesAsync(path, 100, new CancellationToken());

                Console.Write($"One more time? [y/n]");

                var key = Console.ReadKey();

                close = Array.Exists(repeatKeys, c => key.Equals(c));
            }
            while (close);
        }
    }
}
