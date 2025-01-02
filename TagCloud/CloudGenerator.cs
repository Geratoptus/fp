using ResultTools;
using TagCloud.WordsFilter;
using TagCloud.WordsReader;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;

namespace TagCloud;

public class CloudGenerator(
    IImageSaver saver,
    IWordsReader reader,
    BitmapGenerator imageGenerator,
    IEnumerable<IWordsFilter> filters)
{
    private const int MinFontSize = 10;
    private const int MaxFontSize = 80;
    public Result<string> GenerateTagCloud() 
#pragma warning disable CA1416
        => reader
            .ReadWords()
            .Then(BuildFreqDict)
            .Then(ToWordTagList)
            .Then(imageGenerator.GenerateWindowsBitmap)
            .Then(saver.Save);
#pragma warning restore CA1416

    private static Result<List<WordTag>> ToWordTagList(Dictionary<string, int> freqDict)
        => freqDict.Values.Max().AsResult().Then(
            m => freqDict.Select(p => ToWordTag(p, m)).ToList());

    private Result<Dictionary<string, int>> BuildFreqDict(List<string> words) 
        => ApplyFilters(words).Then(wl => wl
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count()));

    private Result<List<string>> ApplyFilters(List<string> words)
        => filters.Aggregate(words.AsResult(), (c, f) => c.Then(f.ApplyFilter));

    private static int TransformFreqToSize(int freq, int maxFreq) 
        => (int)(MinFontSize + (float)freq / maxFreq * (MaxFontSize - MinFontSize));

    private static WordTag ToWordTag(KeyValuePair<string, int> pair, int maxFreq)
        => new(pair.Key, TransformFreqToSize(pair.Value, maxFreq));
}