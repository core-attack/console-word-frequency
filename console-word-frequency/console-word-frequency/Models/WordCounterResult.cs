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

        public WordCounterResult(string outputFile)
        {
            OutputFile = outputFile;
        }

        public WordCounterResult()
        {
        }

        public string OutputFile { get; private set; }

        public virtual IOrderedEnumerable<KeyValuePair<string, long>> SortedWords { get; set; }

        public virtual Dictionary<string, long> Words { get; private set; } = new Dictionary<string, long>();

        public virtual string GetPrettyStatistic()
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

        public virtual void SetOutputFile(string file)
        {
            OutputFile = file;
        }

        public virtual void SetWords(Dictionary<string, long> words)
        {
            Words = words;
        }
    }
}
