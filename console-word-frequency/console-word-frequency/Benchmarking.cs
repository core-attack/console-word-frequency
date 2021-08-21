using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace console_word_frequency
{
    [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(RuntimeMoniker.CoreRt30)]
    [SimpleJob(RuntimeMoniker.Mono)]
    [RPlotExporter]
    public class Benchmarking
    {
        private static string DefaultPath = "files";
        private static string DefaultOutputFile = "output";
        private IWordCounter counter = new TxtWordCounter();
        private IWordCounter counterConc = new TxtWordCounterConcurrent();

        [Benchmark]
        public async Task Base() => await counter.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());

        [Benchmark]
        public async Task Concurrent() => await counterConc.CountWords(DefaultPath, DefaultOutputFile, new CancellationToken());
    }
}
