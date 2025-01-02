using System.Drawing;
using ResultTools;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}