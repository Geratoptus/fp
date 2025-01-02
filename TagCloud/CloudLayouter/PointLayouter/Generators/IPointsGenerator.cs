using System.Drawing;
using ResultTools;

namespace TagCloud.CloudLayouter.PointLayouter.Generators;

public interface IPointsGenerator
{
   public Result<IEnumerable<Point>> GeneratePoints(Point startPoint); 
}