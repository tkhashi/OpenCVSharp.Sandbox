using InsideContours;
using OpenCvSharp;

// 0. 元になる画像(任意の図形が白く、背景は黒く描かれた白黒画像)を受け取る
// 1. 0の画像から、画像の輪郭を抽出
// 2. 0の画像から、輪郭の内側を白く塗った画像を別途生成する
// 3. 2で生成した画像から、1で抽出した輪郭箇所を黒く塗った画像を生成する
// 4. 3で生成した画像から、輪郭を抽出する
// 5. 4の輪郭部を示した画像を表示する
// 6. 4で生成した画像をパラメータにして2に戻る
static Mat NextContours(Mat src)
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

var shape = new ShapeCreator();
const int width = 640;
const int height = 480;
var points = shape.CalculatePolygonPoints(7, width / 2, height / 2, Math.Min(width, height) / 4);

//var image = shape.DrawTrapezoid(width, height);
//var image = shape.DrawEllipse(width, height);
var image = shape.DrawPolygon(width, height, points);
for (var i = 0; i < 10; i++)
{
    image = NextContours(image);
    Cv2.ImShow("Contours", image);
    Cv2.WaitKey(0);
}
