using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Models
{
    public class CalculationContext
    {
        public Size SheetSizeInCells { get; init; }

        public bool InSheet(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X <= SheetSizeInCells.Width && point.Y <= SheetSizeInCells.Height;
        }
        
        public Point[] PointsFrom { get; init; }
    }
}