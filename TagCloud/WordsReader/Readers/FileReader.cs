using System.Text;
using TagCloud.WordsReader.Settings;

namespace TagCloud.WordsReader.Readers;

public class FileReader(string path, Encoding encoding) : IWordsReader
{
    public FileReader(FileReaderSettings settings)
        : this(settings.FilePath, settings.Encoding)
    {
    }
    
    public List<string> ReadWords() 
        => File.ReadAllLines(path, encoding)
            .Select(line => line.Split(" "))
            .SelectMany(arr => arr)
            .ToList();
}