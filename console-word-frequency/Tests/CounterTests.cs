using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ConsoleWordFrequency.Counters;
using ConsoleWordFrequency.Generators;
using ConsoleWordFrequency.Models;
using Xunit;
using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace Tests
{
    public class CounterTests
    {
        private readonly IWordCounter _counter;
        private readonly IFileGenerator _generator;

        public CounterTests()
        {
            _counter = new TxtWordCounterConcurrent();
            _generator = new TxtFileGenerator();
        }

        public static IEnumerable<object[]> Queries
        {
            get
            {
                yield return new object[] { 
                    "files", 
                    "one two three", 
                    "output", 
                    new WordCounterResult("output", new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>()
                    {
                        new ("ONE", 1),
                        new ("TWO", 1),
                        new ("THREE", 1)
                    })), 
                    new CancellationToken() };

                yield return new object[] {
                    "files",
                    "one two three ONE oNe\none\nTwo",
                    "output",
                    new WordCounterResult("output", new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>()
                    {
                        new ("ONE", 4),
                        new ("TWO", 2),
                        new ("THREE", 1)
                    })),
                    new CancellationToken() };

                yield return new object[] {
                    "files",
                    "ONE",
                    "output",
                    new WordCounterResult("output", new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>()
                    {
                        new ("ONE", 1)
                    })),
                    new CancellationToken() };

                yield return new object[] {
                    "files",
                    "one",
                    "output",
                    new WordCounterResult("output", new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>()
                    {
                        new ("ONE", 1)
                    })),
                    new CancellationToken() };

                yield return new object[] {
                    "files",
                    "",
                    "output",
                    new WordCounterResult("output", new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>())),
                    new CancellationToken() };
            }
        }

        [Theory]
        [AutoData]
        public async Task GivenNotExistedPath_WhenCountWordsIsCalled_ThenPathCreated(string output)
        {
            var notExistedPath = "not-existed-path";

            if (Directory.Exists(notExistedPath))
            {
                Directory.Delete(notExistedPath);
            }

            Directory.Exists(notExistedPath).Should().BeFalse();
            
            await _counter.CountWords(notExistedPath, output, new CancellationToken());
            
            Directory.Exists(notExistedPath).Should().BeTrue();
        }

        [Theory]
        [MemberAutoMockData(nameof(Queries))]
        public async Task GivenNotExistedOutput_WhenCountWordsIsCalled_ThenOutputCreated(string path, string content, string output, WordCounterResult expectedResult, CancellationToken cancellationToken)
        {
            var fileName = await _generator.GenerateFileAsync(path, content, cancellationToken);
            var result = await _counter.CountWords(path, output, cancellationToken);

            foreach (var resultConcurrentWord in result.ConcurrentWords)
            {
                expectedResult.ConcurrentWords[resultConcurrentWord.Key].Should().Be(resultConcurrentWord.Value);
            }

            File.Delete(fileName);
        }
    }
}
