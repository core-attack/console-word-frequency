using System.Linq;
using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Sorters
{
    public class WordSorter : IWordSorter
    {
        public async Task<WordCounterResult> Sort(WordCounterResult result)
        {
            result.SortedWords = result.Words.Any() 
                ? result.Words.OrderByDescending(x => x.Value) 
                : result.ConcurrentWords.OrderByDescending(x => x.Value);

            return result;
        }
    }
}
