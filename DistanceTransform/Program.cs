using InsideContours;
using OpenCvSharp;

const int w = 640;
const int h = 480;
var points = ShapeCreator.CalculatePolygonPoints(5, w / 2, h / 2, h / 4);
//using var src = ShapeCreator.DrawTrapezoid(w, h);
//using var src = ShapeCreator.DrawEllipse(w, h);
using var src = ShapeCreator.DrawPolygon(w, h, points);
var contours = src.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxNone);
var binaryImage = src.Clone();
Cv2.DrawContours(binaryImage, contours, -1, new Scalar(255), -1);

// 距離変換を適用する
var dist = binaryImage.DistanceTransform(DistanceTypes.C, DistanceTransformMasks.Mask3);
dist.MinMaxLoc(out double _, out var maxVal);

// 出力画像の準備
var output = binaryImage.CvtColor(ColorConversionCodes.GRAY2BGR);
var random = new Random();
// 距離値ごとに色を変えて描画する（パラレル処理）
Parallel.For(0, (int) maxVal + 1, distance =>
{
    var color = new Vec3b(
        (byte) random.Next(256),
        (byte) random.Next(256),
        (byte) random.Next(256));

    for (var y = 0; y < dist.Rows; y++)
    for (var x = 0; x < dist.Cols; x++)
    {
        if ((int) dist.At<float>(y, x) != distance) continue;
        output.Set(y, x, color);
    }
});

// 結果を表示する
Cv2.ImShow("Color Coded Distance Map", output);
Cv2.WaitKey(0);
