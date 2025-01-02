using FluentAssertions;
using TagCloud.WordsFilter;

namespace TagCloudTests.WordsFilter;

[TestFixture]
[TestOf(typeof(BoringWordsFilter))]
public class BoringWordsFilterTest
{
    private readonly BoringWordsFilter filter = new();

    [Test]
    public void BoringWordsFilter_ApplyFilter_ShouldRemovePrimitiveWords()
    {
        List<string> words = ["a", "the", "hello"];
        var filtered = filter.ApplyFilter(words).GetValueOrThrow();
        filtered.Should().BeEquivalentTo(["hello"], options => options.WithStrictOrdering());
    }
}