using System.Drawing;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using TagCloud;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.Generators;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsFilter;
using TagCloud.WordsReader.Readers;

namespace TagCloudTests;

[TestFixture]
[UseReporter(typeof(RiderReporter))]
[ApprovalTests.Namers.UseApprovalSubdirectory("Samples/Snapshots")]
public class CloudGeneratorTest
{
    [Test]
    public void CloudGenerator_GenerateTagCloud_ShouldGenerateCorrectFile()
    {
        var generator = InitGenerator();

        var savePath = generator.GenerateTagCloud();

        File.Exists(savePath).Should().BeTrue();
        Approvals.VerifyFile(savePath);
    }

    private static CloudGenerator InitGenerator()
    {
        var fileReader = new FileReader("Samples/sample.txt", Encoding.UTF8);
        var imageSaver = new BitmapFileSaver("test", "png");
        var layouter = new CircularCloudLayouter(
            new Point(1920 / 2, 1080 / 2), new FermatSpiralPointsGenerator(1, 0.5));
        var imageGenerator = new BitmapGenerator(
#pragma warning disable CA1416
            new Size(1920, 1080), new FontFamily("Arial"), 
#pragma warning restore CA1416
            Color.Black, Color.Azure, layouter);
        List<IWordsFilter> filters = [new LowercaseFilter(), new BoringWordsFilter()];

        return new CloudGenerator(imageSaver, fileReader, imageGenerator, filters);
    }
}