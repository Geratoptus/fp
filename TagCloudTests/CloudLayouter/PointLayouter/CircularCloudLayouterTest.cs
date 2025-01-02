using System.Diagnostics;
using System.Drawing;
using FluentAssertions;
using TagCloud.CloudLayouter.Extensions;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.Generators;

namespace TagCloudTests.CloudLayouter.PointLayouter;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{
    private readonly Random _randomizer = new();
    private Rectangle[]? _testRectangles;
    private Point _layoutCenter;
    
    [Test]
    public void PutNextRectangle_ShouldPutRectangle()
    {
        _testRectangles = [];
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        var rectangleSize = _randomizer.RandomSize();

        var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        _testRectangles = [rectangle];
        
        GetLayoutSize(_testRectangles).Should().Be(rectangleSize);
    }


    [Test]
    public void PutNextRectangle_ShouldThrowInvalidOperationException_IfFiniteGenerator()
    {
        var finiteGenerator = new FinitePointsGenerator(0);
        var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0), finiteGenerator);
        var invoke = () => circularCloudLayouter.PutNextRectangle(new Size(1, 1));
        
        invoke.Should().Throw<InvalidOperationException>();
    }


    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectangleInCenter_IfFirstInvoke()
    {
        _testRectangles = [];
        var rectangleSize = _randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();

        var actualRectangle = circularCloudLayouter
            .PutNextRectangle(rectangleSize);
        var expectedRectangle = new Rectangle()
            .CreateRectangleWithCenter(_layoutCenter, rectangleSize);
        
        _testRectangles = [actualRectangle];
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }


    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithRightSize()
    {
        _testRectangles = [];
        var rectangleSize = _randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        var actualRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);

        _testRectangles = [actualRectangle];
        actualRectangle.Size.Should().Be(rectangleSize);
    }


    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        _testRectangles = [];
        var rectanglesNumber = _randomizer.Next(100, 250);
        var circularCloudLayouter = SetupLayouterWithRandomParameters();


        _testRectangles = PutRectanglesInLayouter(rectanglesNumber, circularCloudLayouter);


        IsIntersectionBetweenRectangles(_testRectangles).Should().BeFalse();
    }


    [Test]
    [Repeat(10)]
    public void ShouldGenerateLayoutThatHasHighTightnessAndShapeOfCircularCloud_WhenOptimalParametersAreUsed()
    {
        _testRectangles = [];
        const double allowableDelta = 0.38;

        var circularCloudLayouter = SetupLayouterWithOptimalParameters(); 
        _testRectangles = PutRectanglesInLayouter(_randomizer.Next(800, 1200), circularCloudLayouter);

        Debug.Assert(_testRectangles != null, nameof(_testRectangles) + " != null");
        var circumcircleRadius = _testRectangles
            .Max(r => r
                .GetDistanceToMostRemoteCorner(_layoutCenter));
        var circumcircleArea = Math.PI * Math.Pow(circumcircleRadius, 2);
        var rectanglesArea = (double)_testRectangles
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
            .Select(_ => _randomizer.RandomSize(10, 25))
            .Select(circularCloudLayouter.PutNextRectangle)
            .ToArray();
    }


    private CircularCloudLayouter SetupLayouterWithOptimalParameters()
    {
        _layoutCenter = new Point(0, 0);
        return new CircularCloudLayouter(_layoutCenter, 1, 0.5);
    }


    private CircularCloudLayouter SetupLayouterWithRandomParameters()
    {
        var radius = _randomizer.Next(1, 10);
        var angleOffset = _randomizer.Next(1, 10);
        _layoutCenter = _randomizer.RandomPoint(-10, 10);


        return new CircularCloudLayouter(_layoutCenter, radius, angleOffset);
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
        public IEnumerable<Point> GeneratePoints(Point startPoint)
        {
            return Enumerable.Range(0, end)
                .Select(x => new Point(x, x));
        }
    }


}