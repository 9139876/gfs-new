using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Angles;

internal class Circle
{
    private readonly Point _center;
    private readonly double _radius;

    public Circle(Point center, double radius)
    {
        _center = center;
        _radius = radius;
    }

    public bool InCircle(Point point)
        => Math.Pow(Math.Pow(_center.X - point.X, 2) + Math.Pow(_center.Y - point.Y, 2), 0.5) <= _radius;
}