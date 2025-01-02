namespace TagCloud.WordsFilter;

public interface IWordsFilter
{
    List<string> ApplyFilter(List<string> words);
}