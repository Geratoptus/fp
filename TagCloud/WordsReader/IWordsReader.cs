using ResultTools;

namespace TagCloud.WordsReader;

public interface IWordsReader
{
    Result<List<string>> ReadWords();
}