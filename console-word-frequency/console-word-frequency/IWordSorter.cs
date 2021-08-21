using System.Threading.Tasks;

namespace console_word_frequency
{
    public interface IWordSorter
    {
        Task<WordCounterResult> Sort(WordCounterResult result);
    }
}