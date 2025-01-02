using ResultTools;

namespace TagCloud.WordsFilter;

public class LowercaseFilter : IWordsFilter
{
    public Result<List<string>> ApplyFilter(List<string> words) 
        => words
            .Select(w => w.ToLower())
            .ToList();
}