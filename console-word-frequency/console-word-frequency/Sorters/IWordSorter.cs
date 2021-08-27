using System.Collections.Generic;
using System.Linq;

namespace ConsoleWordFrequency.Sorters
{
    public interface IWordSorter
    {
        IOrderedEnumerable<KeyValuePair<string, long>> Sort(IReadOnlyDictionary<string, long> dictionary);
    }
}