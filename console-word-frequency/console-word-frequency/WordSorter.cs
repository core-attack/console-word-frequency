using System.Linq;
using System.Threading.Tasks;

namespace console_word_frequency
{
    public class WordSorter : IWordSorter
    {
        public async Task<WordCounterResult> Sort(WordCounterResult result)
        {
            result.SortedWords = result.Words.OrderByDescending(x => x.Value);
            return result;
        }
    }
}
