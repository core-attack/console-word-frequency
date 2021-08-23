using System.Text;

namespace ConsoleWordFrequency.Generators
{
    public class TxtFileGenerator : FileGenerator, IFileGenerator
    {
        public TxtFileGenerator()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding = Encoding.GetEncoding("windows-1251");
            Extension = "txt";
        }
    }
}
