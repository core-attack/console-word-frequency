using System.Threading;
using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Counters
{
    public interface IWordCounter
    {
        Task<WordCounterResult> CountWords(string path, string output, CancellationToken cancellationToken);

        Task Calculate(string filePath, CancellationToken cancellationToken);
    }
}