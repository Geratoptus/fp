using ResultTools;

namespace TagCloud.WordsFilter;

public class LowercaseFilter : IWordsFilter
{
    public Result<IEnumerable<string>> ApplyFilter(IEnumerable<string> words) 
        => words
            .Select(w => w.ToLower())
            .ToList();
}