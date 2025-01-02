using TagCloud.WordsReader.Settings;
using Xceed.Words.NET;

namespace TagCloud.WordsReader.Readers;

public class WordFileReader(string path) : IWordsReader
{
    public WordFileReader(WordFileReaderSettings settings)
        : this(settings.FilePath)
    { }

    public List<string> ReadWords()
    {
        using var document = DocX.Load(path);
        var paragraphs = document.Paragraphs;
        return paragraphs.Select(p => p.Text).ToList();
    }
}