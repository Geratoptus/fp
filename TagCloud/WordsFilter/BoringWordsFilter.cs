using WeCantSpell.Hunspell;

namespace TagCloud.WordsFilter;

public class BoringWordsFilter : IWordsFilter
{
    private readonly WordList wordList = WordList.CreateFromFiles(
        "./Dictionaries/enUS.dic", 
        "./Dictionaries/enUS.aff");

    public List<string> ApplyFilter(List<string> words)
        => words.Where(w => !IsBoring(w)).ToList();

    private WordEntryDetail[] CheckDetails(string word)
    {
        var details = wordList.CheckDetails(word);
        return wordList[string.IsNullOrEmpty(details.Root) ? word : details.Root];
    }

    private bool IsBoring(string word)
    {
        var details = CheckDetails(word);

        if (details.Length != 0 && details[0].Morphs.Count != 0)
        {
            var po = details[0].Morphs[0];
            return po is "po:pronoun" or "po:preposition" or "po:determiner" or "po:conjunction";
        }
        return false;
    }
}