using DistanceTransform;
using InsideContours;
using OpenCvSharp;

const int w = 640;
const int h = 480;
var points = ShapeCreator.CalculatePolygonPoints(5, w / 2, h / 2, h / 4);
using var src = ShapeCreator.DrawTrapezoid(w, h);
//using var src = ShapeCreator.DrawEllipse(w, h);
//using var src = ShapeCreator.DrawPolygon(w, h, points);
var contours = src.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxNone);
var binaryImage = src.Clone();
Cv2.DrawContours(binaryImage, contours, -1, new Scalar(255), -1);

DistanceTransformer.ShowDistanceColor(binaryImage);
return;

var distanceLabels = DistanceTransformer.GetDistanceLabel(binaryImage);

var maxDistance = distanceLabels.Max(x => x.Distance);
for (var i = 0; i < maxDistance - 1; i++)
{
    var arr = distanceLabels.Where(x => x.Distance == i).ToArray();
    var output = new Mat(h, w, MatType.CV_8UC3);
    foreach (var distanceLabel in arr)
    {
        Cv2.Circle(output, new Point(distanceLabel.X, distanceLabel.Y), 1, Scalar.Red, -1);
    }

    Cv2.ImShow($"Distance: {i}", output);
    Cv2.WaitKey(100);
    Cv2.DestroyAllWindows();
}