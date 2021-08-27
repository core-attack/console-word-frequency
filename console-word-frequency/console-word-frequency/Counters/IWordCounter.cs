using System.Threading;
using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Counters
{
    public interface IWordCounter<T> where T : WordCounterResult, new()
    {
        Task<T> CountWords(string path, string output, CancellationToken cancellationToken);

        Task Calculate(string filePath, CancellationToken cancellationToken);
    }
}