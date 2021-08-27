using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ConsoleWordFrequency.Models;

namespace ConsoleWordFrequency.Counters
{
    public class TxtWordCounter<T> : IWordCounter<T> where T : WordCounterResult, new()
    {
        private readonly Dictionary<string, long> wordsCount;

        public TxtWordCounter()
        {
            wordsCount = new Dictionary<string, long>();
            Extension = ".txt";
        }

        protected string Extension { get; set; }

        public virtual async Task<T> CountWords(string path, string output, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            CheckExt(ref output);

            var files = GetFiles(path);

            foreach (var file in files)
            {
                await Calculate(file, cancellationToken);
            }

            var result = new T();

            result.SetOutputFile(output);
            result.SetWords(wordsCount);

            return result;
        }

        public virtual async Task Calculate(string filePath, CancellationToken cancellationToken)
        {
            using var sr = File.OpenText(filePath);
            var s = string.Empty;

            while ((s = await sr.ReadLineAsync()) != null)
            {
                var words = Regex.Split(s, @"\W+");

                foreach (var word in words)
                {
                    var upper = word.ToUpper();

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
                        wordsCount.Add(upper, 1);
                    }
                }
            }
        }

        protected IEnumerable<string> GetFiles(string path)
        {
            return Directory.EnumerateFiles(path, $"*{Extension}", new EnumerationOptions() { MatchType = MatchType.Simple, RecurseSubdirectories = true });
        }

        protected void CheckExt(ref string output)
        {
            if (!output.EndsWith(Extension))
            {
                output += Extension;
            }
        }
    }
}
