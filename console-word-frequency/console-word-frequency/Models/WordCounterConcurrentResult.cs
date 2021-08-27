using System.Collections.Concurrent;

namespace ConsoleWordFrequency.Models
{
    public class WordCounterConcurrentResult : WordCounterResult
    {
        public WordCounterConcurrentResult(string outputFile, ConcurrentDictionary<string, long> words) : base(outputFile)
        {
            ConcurrentWords = words;
        }
        
        public WordCounterConcurrentResult()
        { }

        public virtual ConcurrentDictionary<string, long> ConcurrentWords { get; private set; } = new ConcurrentDictionary<string, long>();

        public void SetConcurrentWords(ConcurrentDictionary<string, long> words)
        {
            ConcurrentWords = words;
        }
    }
}
