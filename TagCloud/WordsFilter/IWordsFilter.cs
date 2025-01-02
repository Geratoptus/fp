using ResultTools;

namespace TagCloud.WordsFilter;

public interface IWordsFilter
{
    Result<List<string>> ApplyFilter(List<string> words);
}