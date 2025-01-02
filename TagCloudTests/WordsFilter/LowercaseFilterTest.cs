using FluentAssertions;
using TagCloud.WordsFilter;

namespace TagCloudTests.WordsFilter;

[TestFixture]
[TestOf(typeof(LowercaseFilter))]
public class LowercaseFilterTest
{
    private readonly LowercaseFilter _filter = new();

    [Test]
    public void LowercaseFilter_ApplyFilter_ShouldLowerAllWords()
    {
        List<string> words = ["Hello", "WORLD"];
        var filtered = _filter.ApplyFilter(words);
        filtered.Should().BeEquivalentTo(["hello", "world"], options => options.WithStrictOrdering());
    }
}