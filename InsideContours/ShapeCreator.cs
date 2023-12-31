using OpenCvSharp;

namespace InsideContours;

public static class ShapeCreator
{
    // 台形を作る関数
    public static Mat DrawTrapezoid(int width, int height)
    {
        // 黒い背景の画像を作成
        var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
        // 台形の頂点を定義
        var points = new[]
        {
            new Point(width * 0.25, height * 0.5),
            new Point(width * 0.75, height * 0.5),
            new Point(width * 0.6, height * 0.8),
            new Point(width * 0.4, height * 0.8)
        };

        // 白い線で台形を描く
        image.Polylines(new Point[][] { points }, true, new Scalar(255, 255, 255), thickness: 2);

        return image;
    }

    // 楕円
    public static Mat DrawEllipse(int width, int height)
    {
        var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
        var center = new Point(width / 2, height / 2);
        var axes = new Size(width / 4, height / 8);
        image.Ellipse(center, axes, 0, 0, 360, new Scalar(255, 255, 255), 2);
        return image;
    }

    // 半円
    public static Mat DrawSemiCircle(int width, int height)
    {
        var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
        var center = new Point(width / 2, height / 2);
        var radius = Math.Min(width, height) / 4;
        image.Ellipse(center, new Size(radius, radius), 0, -90, 90, new Scalar(255, 255, 255), 2);
        return image;
    }

    // 多角形
    public static Mat DrawPolygon(int width, int height, Point[] points)
    {
        var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
        image.Polylines(new Point[][] { points }, true, new Scalar(255, 255, 255), 2);
        return image;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sides">多角形の辺の数</param>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Point[] CalculatePolygonPoints(int sides, int centerX, int centerY, int radius)
    {
        Point[] points = new Point[sides];
        for (int i = 0; i < sides; i++)
        {
            double angle = 2 * Math.PI / sides * i;
            points[i] = new Point(
                centerX + (int) (radius * Math.Cos(angle)),
                centerY + (int) (radius * Math.Sin(angle))
            );
        }

        return points;
    }
}