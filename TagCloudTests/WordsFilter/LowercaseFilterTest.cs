using FluentAssertions;
using TagCloud.WordsFilter;

namespace TagCloudTests.WordsFilter;

[TestFixture]
[TestOf(typeof(LowercaseFilter))]
public class LowercaseFilterTest
{
    private readonly LowercaseFilter filter = new();

    [Test]
    public void LowercaseFilter_ApplyFilter_ShouldLowerAllWords()
    {
        List<string> words = ["Hello", "WORLD"];
        var filtered = filter.ApplyFilter(words).GetValueOrThrow();
        filtered.Should().BeEquivalentTo(["hello", "world"], options => options.WithStrictOrdering());
    }
}