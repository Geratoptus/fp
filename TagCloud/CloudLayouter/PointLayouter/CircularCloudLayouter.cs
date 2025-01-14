using System.Drawing;
using ResultTools;
using TagCloud.CloudLayouter.Extensions;
using TagCloud.CloudLayouter.PointLayouter.Generators;
using TagCloud.CloudLayouter.PointLayouter.Settings;

namespace TagCloud.CloudLayouter.PointLayouter;

public class CircularCloudLayouter(Point layoutCenter, IPointsGenerator pointsGenerator) : ICloudLayouter
{
    private readonly List<Point> placedPoints = [];
    private readonly List<Rectangle> layoutRectangles = [];

    public CircularCloudLayouter(Point layoutCenter, double radius, double angleOffset) :
        this(layoutCenter, new FermatSpiralPointsGenerator(radius, angleOffset))
    {
    }

    public CircularCloudLayouter(PointLayouterSettings settings)
        : this(settings.Center, settings.Generator)
    {
    }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        => TryPutNext(rectangleSize)
            .Then(RememberRectangle)
            .RefineError("В конструктор CircularCloudLayouter был передан конечный генератор точек");

    private Rectangle RememberRectangle(Rectangle rect)
    {
        layoutRectangles.Add(rect);
        placedPoints.Add(rect.Location - rect.Size / 2);
        return rect;
    }

    private Result<Rectangle> TryPutNext(Size rectangleSize)
        => pointsGenerator
            .GeneratePoints(layoutCenter)
            .Then(pointsEnumerable => pointsEnumerable
                .Except(placedPoints)
                .Select(point => new Rectangle()
                    .CreateRectangleWithCenter(point, rectangleSize))
                .First(r => !layoutRectangles.Any(r.IntersectsWith))
            );
}