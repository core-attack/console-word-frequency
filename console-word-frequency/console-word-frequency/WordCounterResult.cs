using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace console_word_frequency
{
    public class WordCounterResult
    {
        public WordCounterResult(string outputFile, Dictionary<string, int> words)
        {
            OutputFile = outputFile;
            Words = words;
        }

        public string OutputFile { get; private set; }

        public Dictionary<string, int> Words { get; private set; }

        public IOrderedEnumerable<KeyValuePair<string, int>> SortedWords { get; set; }

        public string GetPrettyStatistic()
        {
            if (!Words.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach(var word in SortedWords)
            {
                sb.AppendFormat("{0},{1}\n", word.Key, word.Value);
            }

            return sb.ToString();
        }
    }
}
