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
                .AddTransient<IWordCounter<WordCounterConcurrentResult>, TxtWordCounterConcurrent>()
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
            var counter = serviceProvider.GetService<IWordCounter<WordCounterConcurrentResult>>();
            var sorter = serviceProvider.GetService<IWordSorter>();

            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            if (sorter == null)
            {
                throw new ArgumentNullException(nameof(sorter));
            }

            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            do
            {
                try
                {
                    var path = ReadParam($"Enter the files directory (or '{DefaultPath}' will be used): ", DefaultPath);
                    var outputFileName =
                        ReadParam($"Enter the output file name (or '{DefaultOutputFile}' will be used): ",
                            DefaultOutputFile);

                    //await generator.GenerateFilesAsync(path, 100, 10, 1000, 100, cancellationToken);
                    var result = await counter.CountWords(path, outputFileName, cancellationToken);
                    result.SortedWords = sorter.Sort(result.ConcurrentWords);

                    await WriteResult(result, cancellationToken);

                    Console.WriteLine($"One more time? [y/n]");
                    var key = Console.ReadKey();
                    close = Array.Exists(repeatKeys, c => key.KeyChar.Equals(c));
                }
                catch (PathTooLongException)
                {
                    Console.WriteLine("Path is too long");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine($"It seems that you don't have sufficient privilege. Details: {e.Message}");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Please check path or output file name. Details: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected exception occurred: {e.Message}, {e.StackTrace}");
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
