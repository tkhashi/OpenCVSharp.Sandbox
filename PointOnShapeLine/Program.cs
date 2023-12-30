using InsideContours;
using OpenCvSharp;
using PointOnShapeLine;

var pointOnShape = new PointOnShape();
var shapeCreator = new ShapeCreator();
const int w = 1920;
const int h = 1080;
//var shape = shapeCreator.DrawEllipse(w, h);
////閉じた図形ではない場合、線の周囲を輪郭として取得する
//var shape = shapeCreator.DrawSemiCircle(w, h);
var shape = shapeCreator.DrawTrapezoid(w, h);
//Cv2.ImShow("shape", shape);
//Cv2.WaitKey();

// 点を描画するためのカラー画像を作成
var colorImage = new Mat(h, w, MatType.CV_8UC3);
var points = pointOnShape.GetEquidistantPoints(shape, 100);
if (points == null) throw new NullReferenceException();

// 等間隔の点を描画
foreach (var point in points)
{
    Cv2.Circle(colorImage, point, 1, new Scalar(0, 0, 255), -1); // 赤い円で点を描画
}

// 画像を表示
Cv2.ImShow("Equidistant Points", colorImage);
Cv2.WaitKey(0); // キー入力を待つ
