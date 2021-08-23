using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using ConsoleWordFrequency.Counters;
using ConsoleWordFrequency.Generators;
using ConsoleWordFrequency.Models;
using ConsoleWordFrequency.Sorters;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleWordFrequency
{
    public class Program
    {
        private static string DefaultPath = @"C:\temp\files";
        private static string DefaultOutputFile = @"C:\temp\output";

        static async Task Main()
        {
            //var summary = BenchmarkRunner.Run<Benchmarking>(); //enable benchmarking

            var serviceProvider = new ServiceCollection()
                .AddTransient<IFileGenerator, TxtFileGenerator>()
                .AddTransient<IWordCounter, TxtWordCounterConcurrent>()
                .AddTransient<IWordSorter, WordSorter>()
                .BuildServiceProvider();

            var cancellationToken = new CancellationToken();

            await EntryPoint(serviceProvider, cancellationToken);
        }

        private static async Task EntryPoint(ServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var repeatKeys = new[] { 'y', 'Y', 'н', 'Н' };
            var close = true;
            var generator = serviceProvider.GetService<IFileGenerator>();
            var counter = serviceProvider.GetService<IWordCounter>();
            var sorter = serviceProvider.GetService<IWordSorter>();

            do
            {
                try
                {
                    var path = ReadParam($"Enter the files directory (or '{DefaultPath}' will be used): ", DefaultPath);
                    var outputFileName =
                        ReadParam($"Enter the output file name (or '{DefaultOutputFile}' will be used): ",
                            DefaultOutputFile);

                    //await generator.GenerateFilesAsync(path, 100, 10, 1000, 100,cancellationToken);
                    var unsorted = await counter.CountWords(path, outputFileName, cancellationToken);
                    var sorted = await sorter.Sort(unsorted);

                    await WriteResult(sorted, cancellationToken);

                    Console.WriteLine($"One more time? [y/n]");
                    var key = Console.ReadKey();
                    close = Array.Exists(repeatKeys, c => key.KeyChar.Equals(c));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected exception occured: {e.Message}, {e.StackTrace}");
                }
            } while (close);
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
