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

    protected override Result<List<string>> ReadFromExistingFile(string path)
    {
        try
        {
            return File.ReadAllLines(path, encoding)
                .Select(line => line.Split(" "))
                .SelectMany(arr => arr)
                .ToList();
        }
        catch (ArgumentNullException)
        {
            return Result.Fail<List<string>>("Please provide a valid file path");
        }
        catch (UnauthorizedAccessException)
        {
            return Result.Fail<List<string>>("Access to file denied");
        }
        catch (FormatException)
        {
            return Result.Fail<List<string>>("File format not supported");
        }
    }
}