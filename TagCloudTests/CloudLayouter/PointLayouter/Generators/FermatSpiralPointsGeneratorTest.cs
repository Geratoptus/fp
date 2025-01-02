using System.Drawing;
using FluentAssertions;
using TagCloud.CloudLayouter.Extensions;
using TagCloud.CloudLayouter.PointLayouter.Generators;

namespace TagCloudTests.CloudLayouter.PointLayouter.Generators;

[TestFixture]
[TestOf(typeof(FermatSpiralPointsGenerator))]
public class FermatSpiralPointsGeneratorTest
{
    [TestCase(-1, 1, Description = "Negative radius")]
    [TestCase(0, 1, Description = "Radius is zero")]
    [TestCase(1, 0, Description = "AngleOffset is zero")]
    [TestCase(1, -1, Description = "Negative angleOffset")]
    public void ShouldReturnFailResult_AfterWrongCreation(double radius, double angleOffset)
    {
        var creation = () => new FermatSpiralPointsGenerator(radius, angleOffset)
            .GeneratePoints(new Point());
        creation.Invoke().IsSuccess.Should().BeFalse();

    }

    [TestCaseSource(nameof(_generatePointsTestCases))]
    public Point GeneratePoints_ShouldReturnCorrectPoint(double radius, double angleOffset, int pointNumber)
    {
        var pointsGenerator = new FermatSpiralPointsGenerator(radius, angleOffset);
        var actualPoint = pointsGenerator
            .GeneratePoints(new Point(0, 0))
            .GetValueOrThrow()
            .Skip(pointNumber)
            .First();
        return actualPoint;
    }

    private static TestCaseData[] _generatePointsTestCases =
    [
        new TestCaseData(1, 125, 0).Returns(new Point(0, 0)),
        new TestCaseData(10, 125, 1).Returns(new Point(-2, 3)),
        new TestCaseData(1, 360, 1).Returns(new Point(1, 0)),
        new TestCaseData(2, 180, 1).Returns(new Point(-1, 0)),
        new TestCaseData(4, 90, 1).Returns(new Point(0, 1)),
        new TestCaseData(4, 300, 1).Returns(new Point(2, -3))
    ];
}