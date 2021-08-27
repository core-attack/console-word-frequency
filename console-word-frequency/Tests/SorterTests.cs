using System.Collections.Generic;
using System.Linq;
using ConsoleWordFrequency.Generators;
using ConsoleWordFrequency.Sorters;
using Xunit;
using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace Tests
{
    public class SorterTests
    {
        private readonly IWordSorter _sorter;
        private readonly IFileGenerator _generator;

        public SorterTests()
        {
            _sorter = new WordSorter();
            _generator = new TxtFileGenerator();
        }

        public static IEnumerable<object[]> Queries
        {
            get
            {
                yield return new object[] { 
                    new []
                    {
                        new KeyValuePair<string,long>("one", 23), 
                        new KeyValuePair<string,long>("one2", 64), 
                        new KeyValuePair<string,long>("one3", 63), 
                        new KeyValuePair<string,long>("one4", 58), 
                        new KeyValuePair<string,long>("one5", 37), 
                        new KeyValuePair<string,long>("one6", 39), 
                        new KeyValuePair<string,long>("one7", 35), 
                        new KeyValuePair<string,long>("one8", 96)
                    }.ToDictionary(x => x.Key, x => x.Value),
                    new []
                    {
                        96,
                        64,
                        63,
                        58,
                        39,
                        37,
                        35,
                        23,
                    },
                };
                
            }
        }

        [Theory]
        [MemberAutoMockData(nameof(Queries))]
        public void GivenDictionaryWithData_WhenSortIsCalled_ThenOutputSorted(Dictionary<string, long> input, int[] expectedResult)
        {
            var result = _sorter.Sort(input);

            input.Count.Should().Be(expectedResult.Length);

            var i = 0;

            foreach (var keyValuePair in result)
            {
                keyValuePair.Value.Should().Be(expectedResult[i]);
                i++;
            }
        }
    }
}
