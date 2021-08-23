using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleWordFrequency.Models
{
    public class WordCounterResult
    {
        public WordCounterResult(string outputFile, Dictionary<string, long> words)
        {
            OutputFile = outputFile;
            Words = words;
        }

        public WordCounterResult(string outputFile, ConcurrentDictionary<string, long> words)
        {
            OutputFile = outputFile;
            ConcurrentWords = words;
        }

        public string OutputFile { get; private set; }

        public Dictionary<string, long> Words { get; private set; } = new Dictionary<string, long>();

        public ConcurrentDictionary<string, long> ConcurrentWords { get; private set; } =
            new ConcurrentDictionary<string, long>();

        public IOrderedEnumerable<KeyValuePair<string, long>> SortedWords { get; set; }

        public string GetPrettyStatistic()
        {
            if (!SortedWords.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach(var word in SortedWords)
            {
                sb.AppendFormat("{0},{1}\n", word.Key.ToLowerInvariant(), word.Value);
            }

            return sb.ToString();
        }
    }
}
