using ResultTools;
using TagCloud.WordsReader.Settings;
using Xceed.Words.NET;

namespace TagCloud.WordsReader.Readers;

public class WordFileReader(string path) : BaseFileReader(path)
{
    public WordFileReader(WordFileReaderSettings settings)
        : this(settings.FilePath)
    { }

    protected override Result<List<string>> ReadFromExistingFile(string path)
    {
        try
        {
            using var document = DocX.Load(path);
            var paragraphs = document.Paragraphs;
            return paragraphs.Select(p => p.Text).ToList();
        }
        catch (IOException)
        {
            return Result.Fail<List<string>>($"Can't open file {path}");
        }
        catch (NullReferenceException)
        {
            return Result.Fail<List<string>>("Null reference, something went wrong with load");
        }
    }
}