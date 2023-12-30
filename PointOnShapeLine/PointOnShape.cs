using OpenCvSharp;

namespace PointOnShapeLine;

public class PointOnShape
{
    public List<Point> GetEquidistantPoints(Mat image, int count)
    {
        // 輪郭を検出
        var contour = image
            .FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple)
            .MaxBy(c => c.Length);

        // 輪郭の総距離を計算
        double totalLength = 0;
        for (var i = 0; i < contour.Length; i++)
        {
            var start = contour[i];
            var end = contour[(i + 1) % contour.Length];
            totalLength += start.DistanceTo(end);
        }

        // 等間隔の距離
        double step = totalLength / count;

        // 等間隔の点を取得
        List<Point> equidistantPoints = new List<Point>();
        double currentDistance = 0;
        for (int i = 0, j = 0; j < count; i = (i + 1) % contour.Length)
        {
            var start = contour[i];
            var end = contour[(i + 1) % contour.Length];
            var segmentLength = start.DistanceTo(end);

            // 次の等間隔点に達した場合
            while (currentDistance + segmentLength >= j * step && j < count)
            {
                var ratio = (j * step - currentDistance) / segmentLength;
                var equidistantPoint = new Point(
                    start.X + ratio * (end.X - start.X),
                    start.Y + ratio * (end.Y - start.Y));
                equidistantPoints.Add(equidistantPoint);
                j++;
            }

            currentDistance += segmentLength;
        }

        return equidistantPoints;
    }
}