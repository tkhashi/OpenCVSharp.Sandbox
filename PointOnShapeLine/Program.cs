using InsideContours;
using OpenCvSharp;

var shapeCreator = new ShapeCreator();
const int w = 1940;
const int h = 1280;
//var shape = shapeCreator.DrawEllipse(w, h);
////閉じた図形ではない場合、線の周囲を輪郭として取得する
//var shape = shapeCreator.DrawSemiCircle(w, h);
var shape = shapeCreator.DrawTrapezoid(w, h);
//Cv2.ImShow("shape", shape);
//Cv2.WaitKey();

// 点を描画するためのカラー画像を作成
var colorImage = new Mat(h, w, MatType.CV_8UC3);
var points = GetEquidistantPoints(shape, 100);

// 等間隔の点を描画
foreach (var point in points)
{
    Cv2.Circle(colorImage, point, 1, new Scalar(0, 0, 255), -1); // 赤い円で点を描画
}

// 画像を表示
Cv2.ImShow("Equidistant Points", colorImage);
Cv2.WaitKey(0); // キー入力を待つ

static List<Point> GetEquidistantPoints(Mat image, int count)
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