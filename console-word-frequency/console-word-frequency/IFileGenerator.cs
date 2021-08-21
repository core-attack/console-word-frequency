using System.Threading;
using System.Threading.Tasks;

namespace console_word_frequency
{
    public interface IFileGenerator
    {
        Task GenerateFileAsync(string path, CancellationToken cancellationToken);
    }
}