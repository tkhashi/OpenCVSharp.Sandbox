using OpenCvSharp;
using PointOnShapeLine;

namespace Main;

public class Shape : IDisposable
{
    private readonly Mat _shapeMat;
    private readonly PointOnShape _pointOnShape = new();
    private readonly InsideContours.InsideContours _insideContours = new();
    public Point Center { get; }
    public int MajorAxis { get; }
    public int MinorAxis { get; }

    public Shape(Mat shapeMat, Point center, int majorAxis, int minorAxis)
    {
        _shapeMat = shapeMat;
        Center = center;
        MajorAxis = majorAxis;
        MinorAxis = minorAxis;
    }

    public Shape Init(Point center, int majorAxis, int minorAxis)
    {
        // 内側の図形のShapeを返す
        var nextMat = _insideContours.NextContours(_shapeMat);
        var newShape = new Shape(nextMat, center, majorAxis, minorAxis);
        return newShape;
    }

    public Point[]? GetPoints(int splitNum)
    {
        var points = _pointOnShape.GetEquidistantPoints(_shapeMat, splitNum)?.ToArray() ?? null;

        return points;
    }

    public void Dispose()
    {
        _shapeMat.Dispose();
    }
}