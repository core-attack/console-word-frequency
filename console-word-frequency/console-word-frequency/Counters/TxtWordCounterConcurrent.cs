using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Counters
{
    public class TxtWordCounterConcurrent : TxtWordCounter
    {
        private readonly ConcurrentDictionary<string, long> wordsCount;

        public TxtWordCounterConcurrent ()
        {
            wordsCount = new ConcurrentDictionary<string, long>();
        }

        public override async Task<WordCounterResult> CountWords(string path, string output, CancellationToken cancellationToken)
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
                tasks.Add(Calculate(file, cancellationToken));
            }

            await Task.WhenAll(tasks);

            return new WordCounterResult(output, wordsCount);
        }

        public override async Task Calculate(string filePath, CancellationToken cancellationToken)
        {
            using var sr = File.OpenText(filePath);
            var s = string.Empty;

            while ((s = await sr.ReadLineAsync()) != null)
            {
                var words = Regex.Split(s, @"\W+");

                foreach (var word in words)
                {
                    var upper = word.ToUpperInvariant();

                    if (string.IsNullOrEmpty(upper))
                    {
                        continue;
                    }

                    if (wordsCount.ContainsKey(upper))
                    {
                        wordsCount[upper]++;
                    }
                    else
                    {
                        wordsCount.TryAdd(upper, 1);
                    }
                }
            }
        }
    }
}
