using System.Drawing;

namespace TagCloud.CloudLayouter.PointLayouter.Generators;

public interface IPointsGenerator
{
   public IEnumerable<Point> GeneratePoints(Point startPoint); 
}