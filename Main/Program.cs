using Main;
using InsideContours;
using OpenCvSharp;

// 任意の奥行き(px)数分内側の図形を取得

const int w = 640 * 2;
const int h = 480 * 2;
var shapeObj = ShapeCreator.DrawEllipse(w, h);
var shape = new Shape(shapeObj, new Point(w / 2, h / 2), w / 2, h / 2);

const int count = 100;
const int splitNum = 720 / 10;
for (var i = 0; i < count; i++)
{
    // 内側がなくなったらreturn
    var points = shape.GetPoints(splitNum);
    if (points == null) return;

    // 点を描画するためのカラー画像を作成
    var colorImage = new Mat(h, w, MatType.CV_8UC3);
    // 等間隔の点を描画
    foreach (var point in points)
    {
        Cv2.Circle(colorImage, point, 1, new Scalar(0, 0, 255), -1); // 赤い円で点を描画
    }

    // 画像を表示
    Cv2.ImShow("Equidistant Points", colorImage);
    Cv2.WaitKey(50); // キー入力を待つ

    var newShape = shape.Init(shape.Center, shape.MajorAxis, shape.MinorAxis);
    shape.Dispose();
    shape = newShape;
}