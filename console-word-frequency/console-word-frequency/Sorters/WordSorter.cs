using System.Collections.Generic;
using System.Linq;

namespace ConsoleWordFrequency.Sorters
{
    public class WordSorter : IWordSorter
    {
        public IOrderedEnumerable<KeyValuePair<string, long>> Sort(IReadOnlyDictionary<string, long> dictionary)
        {
            return dictionary.OrderByDescending(x => x.Value);
        }
    }
}
