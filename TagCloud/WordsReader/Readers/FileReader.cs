using System.Text;
using ResultTools;
using TagCloud.WordsReader.Settings;

namespace TagCloud.WordsReader.Readers;

public class FileReader(string path, Encoding encoding) : BaseFileReader(path)
{
    public FileReader(FileReaderSettings settings)
        : this(settings.FilePath, settings.Encoding)
    {
    }
    
    public override Result<List<string>> ReadFromExistingFile(string path)
        => File.ReadAllLines(path, encoding)
            .Select(line => line.Split(" "))
            .SelectMany(arr => arr)
            .ToList();
}