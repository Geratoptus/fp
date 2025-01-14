using ResultTools;
using WeCantSpell.Hunspell;

namespace TagCloud.WordsFilter;

public class BoringWordsFilter : IWordsFilter
{
    private const string EnglishDic = "./Dictionaries/enUS.dic";
    private const string EnglishAff = "./Dictionaries/enUS.aff";
    public Result<IEnumerable<string>> ApplyFilter(IEnumerable<string> words)
        => File.Exists(EnglishDic) && File.Exists(EnglishAff) 
            ? WordList.CreateFromFiles(EnglishDic, EnglishAff)
                .AsResult()
                .Then(wl => words.Where(w => !IsBoring(w, wl)))
            : Result.Fail<IEnumerable<string>>("Cannot find dictionaries");

    private static WordEntryDetail[] CheckDetails(string word, WordList wordList)
    {
        var details = wordList.CheckDetails(word);
        return wordList[string.IsNullOrEmpty(details.Root) ? word : details.Root];
    }

    private static bool IsBoring(string word, WordList wordList)
    {
        var details = CheckDetails(word, wordList);

        if (details.Length == 0 || details[0].Morphs.Count == 0) return false;
        var po = details[0].Morphs[0];
        return po is "po:pronoun" or "po:preposition" or "po:determiner" or "po:conjunction";
    }
}