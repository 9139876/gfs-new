using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Models
{
    public class CalculationContext
    {
        /// <summary>
        /// Размер листа в клеточках
        /// </summary>
        public Size SheetSizeInCells { get; init; }

        /// <summary>
        /// Разброс прогноза 
        /// </summary>
        public byte ForecastSpread { get; init; }
        
        /// <summary>
        /// Коллекция точек,от которых делается прогноз 
        /// </summary>
        public Point[] PointsFrom { get; init; }
        
        /// <summary>
        /// Проверяет, попадает ли точка на лист
        /// </summary>
        /// <param name="point">Точка</param>
        public bool InSheet(Point point)
        {
            return point.X > 0 && point.Y > 0 && point.X < SheetSizeInCells.Width && point.Y < SheetSizeInCells.Height;
        }
    }
}