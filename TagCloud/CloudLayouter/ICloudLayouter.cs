using System.Drawing;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}