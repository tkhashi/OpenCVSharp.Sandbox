using InsideContours;
using OpenCvSharp;

var shape = new ShapeCreator();
var contours = new InsideContours.InsideContours();
const int width = 640;
const int height = 480;
var points = shape.CalculatePolygonPoints(7, width / 2, height / 2, Math.Min(width, height) / 4);

//var image = _shape.DrawTrapezoid(width, height);
//var image = _shape.DrawEllipse(width, height);
var image = shape.DrawPolygon(width, height, points);
for (var i = 0; i < 50; i++)
{
    var newImage = contours.NextContours(image);
    image.Dispose();
    image = newImage;
    Cv2.ImShow("Contours", image);
    Cv2.WaitKey(0);
}