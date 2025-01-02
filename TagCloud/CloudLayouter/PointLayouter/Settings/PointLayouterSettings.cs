using System.Drawing;
using TagCloud.CloudLayouter.PointLayouter.Generators;

namespace TagCloud.CloudLayouter.PointLayouter.Settings;

public record PointLayouterSettings(Point Center, IPointsGenerator Generator);