using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace console_word_frequency
{
    public class TxtWordCounterConcurrent : IWordCounter
    {
        private string Extension = ".txt";
        private readonly ConcurrentDictionary<string, int> wordsCount;

        public TxtWordCounterConcurrent ()
        {
            wordsCount = new ConcurrentDictionary<string, int>();
        }

        public async Task<WordCounterResult> CountWords(string path, string output, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!output.EndsWith(Extension))
            {
                output += Extension;
            }

            var files = Directory.EnumerateFiles(path, $"*{Extension}", new EnumerationOptions() { MatchType = MatchType.Simple, RecurseSubdirectories = true });
            var tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Calculate(file));
            }

            await Task.WhenAll(tasks);

            return new WordCounterResult(output, wordsCount);
        }

        private async Task Calculate(string filePath)
        {
            var s = await File.ReadAllTextAsync(filePath);
            var words = Regex.Split(s, @"\W+");

            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                if (wordsCount.ContainsKey(word))
                {
                    wordsCount[word]++;
                }
                else
                {
                    wordsCount.TryAdd(word, 1);
                }
            }
        }
    }
}
