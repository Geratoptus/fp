using System.Drawing;
using TagCloud.CloudLayouter.PointLayouter.Settings.Generators;

namespace TagCloud.CloudLayouter.PointLayouter.Generators;

public class FermatSpiralPointsGenerator : IPointsGenerator
{
    private readonly double _angleOffset;
    private readonly double _radius;

    private double OffsetPerRadian => _radius / (2 * Math.PI);

    public FermatSpiralPointsGenerator(double radius, double angleOffset)
    {
        if (radius <= 0)
            throw new ArgumentException("radius must be greater than 0", nameof(radius));
        if (angleOffset <= 0)
            throw new ArgumentException("angleOffset must be greater than 0", nameof(angleOffset));

        _angleOffset = angleOffset * Math.PI / 180;
        _radius = radius;
    }

    public FermatSpiralPointsGenerator(FermatSpiralSettings settings)
        : this(settings.Radius, settings.AngleOffset)
    {
    }

    public IEnumerable<Point> GeneratePoints(Point spiralCenter)
    {
        double angle = 0;

        while (true)
        {
            yield return GetPointByPolarCoordinates(spiralCenter, angle);
            angle += _angleOffset;
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