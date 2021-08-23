using System.Threading;
using System.Threading.Tasks;

namespace ConsoleWordFrequency.Generators
{
    public interface IFileGenerator
    {
        string GetFileName(string fileName);

        Task GenerateFileAsync(string path, string content, CancellationToken cancellationToken);

        void GenerateFile(string path, string filename, string content);

        Task GenerateFilesAsync(string path, int filesCount, int levels, int wordsCount, int sentencesCount,
            CancellationToken cancellationToken);
    }
}