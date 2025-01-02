using System.Drawing;
using ResultTools;
using TagCloud.CloudLayouter.PointLayouter.Settings.Generators;

namespace TagCloud.CloudLayouter.PointLayouter.Generators;

public class FermatSpiralPointsGenerator(double radius, double angleOffset) : IPointsGenerator
{
    private readonly double angleOffset = angleOffset * Math.PI / 180;

    private double OffsetPerRadian => radius / (2 * Math.PI);

    public FermatSpiralPointsGenerator(FermatSpiralSettings settings)
        : this(settings.Radius, settings.AngleOffset)
    {
    }

    public Result<IEnumerable<Point>> GeneratePoints(Point startPoint)
    {
        if (radius <= 0 || angleOffset <= 0)
        {
            var argName = radius <= 0 ? nameof(radius) : nameof(angleOffset);
            return Result.Fail<IEnumerable<Point>>($"Fermat spiral params should be positive: {argName}");
        }
        return PointGenerator(startPoint).AsResult();
    }
    
    public IEnumerable<Point> PointGenerator(Point spiralCenter)
    {
        double angle = 0;

        while (true)
        {
            yield return GetPointByPolarCoordinates(spiralCenter, angle);
            angle += angleOffset;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private Point GetPointByPolarCoordinates(Point spiralCenter, double angle)
    {
        var radiusVector = OffsetPerRadian * angle;

        var x = (int)Math.Round(
            radiusVector * Math.Cos(angle) + spiralCenter.X);
        var y = (int)Math.Round(
            radiusVector * Math.Sin(angle) + spiralCenter.Y);

        return new Point(x, y);
    }
}