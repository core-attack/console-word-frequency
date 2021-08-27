using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ConsoleWordFrequency.Counters;
using ConsoleWordFrequency.Generators;
using ConsoleWordFrequency.Models;
using ConsoleWordFrequency.Sorters;
using LoremNET;

namespace ConsoleWordFrequency
{
    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp30)]
    //[SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net50)]
    //[SimpleJob(RuntimeMoniker.CoreRt50)]
    //[SimpleJob(RuntimeMoniker.CoreRt60)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    //[RPlotExporter]
    public class Benchmarking
    {
        private static string DefaultPath = @"C:\temp\files";
        private static string DefaultOutputFile = "output";
        private static string DefaultInputFileName = "input";
        private readonly IWordCounter<WordCounterResult> _counter;
        private readonly IWordCounter<WordCounterConcurrentResult> _counterConc;
        private readonly IFileGenerator _generator;
        private readonly IWordSorter _sorter;
        private readonly WordCounterConcurrentResult Result;

        public Benchmarking()
        {
            _generator = new TxtFileGenerator();
            _counter = new TxtWordCounter<WordCounterResult>();
            _counterConc = new TxtWordCounterConcurrent();
            _sorter = new WordSorter();

            //var content = Lorem.Paragraph(10000, 100);

            //var sb = new StringBuilder();

            //for (int i = 0; i < 10000; i++)
            //{
            //    sb.Append($"value{i} ");
            //}

            //_generator.GenerateFile(DefaultPath, DefaultInputFileName, sb.ToString());

            Result = _counterConc.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken()).Result;
        }
    

        //[Benchmark]
        public async Task Base() => await _counter.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        //[Benchmark]
        public async Task Concurrent() => await _counterConc.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        //[Benchmark]
        public async Task Split() => await _counter.Calculate(Path.Combine(DefaultPath, _generator.GetFileName(DefaultInputFileName)), new CancellationToken());

        //[Benchmark]
        public async Task Regex() => await _counterConc.Calculate(Path.Combine(DefaultPath, _generator.GetFileName(DefaultInputFileName)), new CancellationToken());

        //[Benchmark]
        public void Sort() => _sorter.Sort(Result.ConcurrentWords);
    }
}
