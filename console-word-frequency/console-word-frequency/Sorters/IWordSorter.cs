using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Sorters
{
    public interface IWordSorter
    {
        Task<WordCounterResult> Sort(WordCounterResult result);
    }
}