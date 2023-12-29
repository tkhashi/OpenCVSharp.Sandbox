using OpenCvSharp;

// 台形を作る関数
Mat DrawTrapezoid(int width, int height)
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
Mat DrawEllipse(int width, int height)
{
    var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
    var center = new Point(width / 2, height / 2);
    var axes = new Size(width / 4, height / 8);
    image.Ellipse(center, axes, 0, 0, 360, new Scalar(255, 255, 255), 2);
    return image;
}

// 半円
Mat DrawSemiCircle(int width, int height)
{
    var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
    var center = new Point(width / 2, height / 2);
    var radius = Math.Min(width, height) / 4;
    image.Ellipse(center, new Size(radius, radius), 0, -90, 90, new Scalar(255, 255, 255), 2);
    return image;
}

// 多角形
Mat DrawPolygon(int width, int height, Point[] points)
{
    var image = new Mat(height, width, MatType.CV_8UC1, new Scalar(0, 0, 0));
    image.Polylines(new Point[][] { points }, true, new Scalar(255, 255, 255), 2);
    return image;
}

// 0. 元になる画像(任意の図形が白く、背景は黒く描かれた白黒画像)を受け取る
// 1. 0の画像から、画像の輪郭を抽出
// 2. 0の画像から、輪郭の内側を白く塗った画像を別途生成する
// 3. 2で生成した画像から、1で抽出した輪郭箇所を黒く塗った画像を生成する
// 4. 3で生成した画像から、輪郭を抽出する
// 5. 4の輪郭部を示した画像を表示する
// 6. 4で生成した画像をパラメータにして2に戻る
Mat NextContours(Mat src)
{
    // 輪郭の抽出
    Cv2.FindContours(src, out Point[][] contours, out _, RetrievalModes.External,
        ContourApproximationModes.ApproxNone);

    // 輪郭の内側を白で塗りつぶす
    using var filledContours = src.Clone();
    Cv2.DrawContours(filledContours, contours, -1, new Scalar(255), -1);

    // 輪郭部分を黒く塗る
    var newImage = filledContours.Clone();
    Cv2.DrawContours(newImage, contours, -1, new Scalar(0), 2);

    return newImage;
}

var width = 640;
var height = 480;
var points = CalculatePolygonPoints(7, width / 2, height / 2, Math.Min(width, height) / 4);

static Point[] CalculatePolygonPoints(int sides, int centerX, int centerY, int radius)
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

//var image = DrawTrapezoid(width, height);
//var image = DrawEllipse(width, height);
var image = DrawPolygon(width, height, points);
for (var i = 0; i < 10; i++)
{
    image = NextContours(image);
    Cv2.ImShow("Contours", image);
    Cv2.WaitKey(0);
}
