using ResultTools;

namespace TagCloud.WordsFilter;

public interface IWordsFilter
{
    Result<IEnumerable<string>> ApplyFilter(IEnumerable<string> words);
}