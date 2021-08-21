using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace console_word_frequency
{
    public interface IWordCounter
    {
        Task<WordCounterResult> CountWords(string path, string output, CancellationToken cancellationToken);
    }
}