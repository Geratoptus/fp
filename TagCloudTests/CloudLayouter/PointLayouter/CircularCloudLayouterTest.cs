using System.Diagnostics;
using System.Drawing;
using FluentAssertions;
using ResultTools;
using TagCloud.CloudLayouter.Extensions;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.Generators;

namespace TagCloudTests.CloudLayouter.PointLayouter;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{
    private readonly Random randomizer = new();
    private Rectangle[]? testRectangles;
    private Point layoutCenter;
    
    [Test]
    public void PutNextRectangle_ShouldPutRectangle()
    {
        testRectangles = [];
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        var rectangleSize = randomizer.RandomSize();

        var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        testRectangles = [rectangle.GetValueOrThrow()];
        
        GetLayoutSize(testRectangles).Should().Be(rectangleSize);
    }


    [Test]
    public void PutNextRectangle_ShouldThrowInvalidOperationException_IfFiniteGenerator()
    {
        var finiteGenerator = new FinitePointsGenerator(0);
        var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0), finiteGenerator);
        var invoke = () => circularCloudLayouter.PutNextRectangle(new Size(1, 1)).GetValueOrThrow();
        
        invoke.Should().Throw<InvalidOperationException>();
    }


    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectangleInCenter_IfFirstInvoke()
    {
        testRectangles = [];
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();

        var actualRectangle = circularCloudLayouter
            .PutNextRectangle(rectangleSize)
            .GetValueOrThrow();
        var expectedRectangle = new Rectangle()
            .CreateRectangleWithCenter(layoutCenter, rectangleSize);
        
        testRectangles = [actualRectangle];
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }


    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithRightSize()
    {
        testRectangles = [];
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        var actualRectangle = circularCloudLayouter
            .PutNextRectangle(rectangleSize)
            .GetValueOrThrow();

        testRectangles = [actualRectangle];
        actualRectangle.Size.Should().Be(rectangleSize);
    }


    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        testRectangles = [];
        var rectanglesNumber = randomizer.Next(100, 250);
        var circularCloudLayouter = SetupLayouterWithRandomParameters();


        testRectangles = PutRectanglesInLayouter(rectanglesNumber, circularCloudLayouter);


        IsIntersectionBetweenRectangles(testRectangles).Should().BeFalse();
    }


    [Test]
    [Repeat(1)]
    public void ShouldGenerateLayoutThatHasHighTightnessAndShapeOfCircularCloud_WhenOptimalParametersAreUsed()
    {
        testRectangles = [];
        const double allowableDelta = 0.45;

        var circularCloudLayouter = SetupLayouterWithOptimalParameters(); 
        testRectangles = PutRectanglesInLayouter(randomizer.Next(500, 600), circularCloudLayouter);

        Debug.Assert(testRectangles != null, nameof(testRectangles) + " != null");
        var circumcircleRadius = testRectangles
            .Max(r => r
                .GetDistanceToMostRemoteCorner(layoutCenter));
        var circumcircleArea = Math.PI * Math.Pow(circumcircleRadius, 2);
        var rectanglesArea = (double)testRectangles
            .Select(rectangle => rectangle.Height * rectangle.Width)
            .Sum();

        var areasFraction = circumcircleArea / rectanglesArea;
        areasFraction.Should().BeApproximately(1, allowableDelta);        
    }


    private Rectangle[] PutRectanglesInLayouter(int rectanglesNumber, 
        CircularCloudLayouter circularCloudLayouter)
    {
        return Enumerable
            .Range(0, rectanglesNumber)
            .Select(_ => randomizer.RandomSize(10, 25))
            .Select(circularCloudLayouter.PutNextRectangle)
            .Select(result => result.GetValueOrThrow())
            .ToArray();
    }


    private CircularCloudLayouter SetupLayouterWithOptimalParameters()
    {
        layoutCenter = new Point(0, 0);
        return new CircularCloudLayouter(layoutCenter, 1, 0.5);
    }


    private CircularCloudLayouter SetupLayouterWithRandomParameters()
    {
        var radius = randomizer.Next(1, 10);
        var angleOffset = randomizer.Next(1, 10);
        layoutCenter = randomizer.RandomPoint(-10, 10);


        return new CircularCloudLayouter(layoutCenter, radius, angleOffset);
    }


    private bool IsIntersectionBetweenRectangles(Rectangle[]? rectangles)
    {
        Debug.Assert(rectangles != null, nameof(rectangles) + " != null");
        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }


        return false;
    }

    private Size GetLayoutSize(IEnumerable<Rectangle>? rectangles)
    {
        Debug.Assert(rectangles != null, nameof(rectangles) + " != null");
        var enumerable = rectangles.ToList();
        var layoutWidth = enumerable.Max(rectangle => rectangle.Right) - 
                          enumerable.Min(rectangle => rectangle.Left);
        var layoutHeight = enumerable.Max(rectangle => rectangle.Bottom)
                           - enumerable.Min(rectangle => rectangle.Top);
        return new Size(layoutWidth, layoutHeight);
    }


    class FinitePointsGenerator(int end) : IPointsGenerator
    {
        public Result<IEnumerable<Point>> GeneratePoints(Point startPoint)
        {
            return Enumerable.Range(0, end)
                .Select(x => new Point(x, x))
                .AsResult();
        }
    }


}