using FluentAssertions;
using TagCloud.WordsFilter;

namespace TagCloudTests.WordsFilter;

[TestFixture]
[TestOf(typeof(BoringWordsFilter))]
public class BoringWordsFilterTest
{
    private readonly BoringWordsFilter _filter = new();

    [Test]
    public void BoringWordsFilter_ApplyFilter_ShouldRemovePrimitiveWords()
    {
        List<string> words = ["a", "the", "hello"];
        var filtered = _filter.ApplyFilter(words);
        filtered.Should().BeEquivalentTo(["hello"], options => options.WithStrictOrdering());
    }
}