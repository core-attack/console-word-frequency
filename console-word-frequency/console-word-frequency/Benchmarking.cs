using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ConsoleWordFrequency.Counters;
using ConsoleWordFrequency.Generators;
using LoremNET;

namespace ConsoleWordFrequency
{
    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp30)]
    //[SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.Net60)]
    //[SimpleJob(RuntimeMoniker.CoreRt50)]
    //[SimpleJob(RuntimeMoniker.CoreRt60)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    //[RPlotExporter]
    public class Benchmarking
    {
        private static string DefaultPath = @"C:\temp\files";
        private static string DefaultOutputFile = "output";
        private static string DefaultInputFileName = "input";
        private readonly IWordCounter _counter;
        private readonly IWordCounter _counterConc;
        private readonly IFileGenerator _generator;

        public Benchmarking()
        {
            _generator = new TxtFileGenerator();
            _counter = new TxtWordCounter();
            _counterConc = new TxtWordCounterConcurrent();

            var content = Lorem.Paragraph(1000000, 10000);
            _generator.GenerateFile(DefaultPath, DefaultInputFileName, content);
        }
    

        [Benchmark]
        public async Task Base() => await _counter.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        [Benchmark]
        public async Task Concurrent() => await _counterConc.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        //[Benchmark]
        public async Task FileReadAllTextAsync() => await _counter.Calculate(Path.Combine(DefaultPath, _generator.GetFileName(DefaultInputFileName)), new CancellationToken());

        //[Benchmark]
        public async Task FileOpenText() => await _counterConc.Calculate(Path.Combine(DefaultPath, _generator.GetFileName(DefaultInputFileName)), new CancellationToken());
    }
}
