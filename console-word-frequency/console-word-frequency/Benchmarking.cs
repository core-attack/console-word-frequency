using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace console_word_frequency
{
    //[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp30)]
    //[SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net50)]
    //[SimpleJob(RuntimeMoniker.Net60)]
    //[SimpleJob(RuntimeMoniker.CoreRt50)]
    //[SimpleJob(RuntimeMoniker.CoreRt60)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    //[RPlotExporter]
    public class Benchmarking
    {
        private static string DefaultPath = "files";
        private static string DefaultOutputFile = "output";
        private IWordCounter counter = new TxtWordCounter();
        private IWordCounter counterConc = new TxtWordCounterConcurrent();
        private IWordCounter counterParal = new TxtWordCounterParallel();

        [Benchmark]
        public async Task Base() => await counter.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        [Benchmark]
        public async Task Concurrent() => await counterConc.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        [Benchmark]
        public async Task Patallel() => await counterParal.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());
    }
}
