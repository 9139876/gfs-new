using System.Drawing;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
#pragma warning disable CS8618

namespace GFS.AnalysisSystem.Library.Calculation.Models
{
    public class CalculationContext
    {
        /// <summary>
        /// Таймфрейм листа
        /// </summary>
        public TimeFrameEnum TimeFrame{ get; init; }
        
        /// <summary>
        /// Размер листа в клеточках
        /// </summary>
        public Size SheetSizeInCells { get; init; }

        /// <summary>
        /// Значения дат для клеточек
        /// </summary>
        public DateTime[] TimeValues { get; init; }

        /// <summary>
        /// Значения цен для клеточек
        /// </summary>
        public decimal[] PriceValues { get; init; }

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
        /// <param name="pointInCells">Координаты точки в клеточках</param>
        public bool InSheet(Point pointInCells)
        {
            return pointInCells.X > 0 && pointInCells.Y > 0 && pointInCells.X < SheetSizeInCells.Width && pointInCells.Y < SheetSizeInCells.Height;
        }

        /// <summary>
        /// Возвращает позицию в координатах цена время
        /// </summary>
        /// <param name="pointInCells">Координаты точки в клеточках</param>
        public PriceTimePoint GetPriceTimePosition(Point pointInCells)
            => new()
            {
                Price = PriceValues[pointInCells.Y],
                Date = TimeValues[pointInCells.X]
            };
    }
}