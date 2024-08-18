using System.Drawing;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;

#pragma warning disable CS8618

namespace GFS.AnalysisSystem.Library.Calculation.Models
{
    public class CalculationContext
    {
        public CalculationContext(
            TimeFrameEnum timeFrame,
            ForecastWindow forecastWindow,
            DateTime[] cellTimeValues,
            decimal[] cellPriceValues,
            byte forecastSpread, 
            Point[] sourcePoints,
            Point targetPoint, 
            CandleInCells[] candles)
        {
            TimeFrame = timeFrame;
            ForecastWindow = forecastWindow;
            CellTimeValues = cellTimeValues;
            CellPriceValues = cellPriceValues;
            ForecastSpread = forecastSpread;
            SourcePoints = sourcePoints;
            TargetPoint = targetPoint;
            Candles = candles;
        }

        /// <summary>
        /// Таймфрейм листа
        /// </summary>
        public TimeFrameEnum TimeFrame { get; }

        /// <summary>
        /// Окно прогнозирования
        /// </summary>
        public ForecastWindow ForecastWindow { get; }

        /// <summary>
        /// Значения дат для клеточек
        /// </summary>
        public DateTime[] CellTimeValues { get; }

        /// <summary>
        /// Значения цен для клеточек
        /// </summary>
        public decimal[] CellPriceValues { get; }

        /// <summary>
        /// Предыдущие котировки
        /// </summary>
        public CandleInCells[] Candles { get; }
        
        /// <summary>
        /// Разброс прогноза 
        /// </summary>
        public byte ForecastSpread { get; }

        /// <summary>
        /// Коллекция точек, которые указывают на целевую точку
        /// </summary>
        public Point[] SourcePoints { get; }
        
        /// <summary>
        /// Целевая точка
        /// </summary>
        public Point TargetPoint{ get; }

        /// <summary>
        /// Возвращает позицию в координатах цена время
        /// </summary>
        /// <param name="pointInCells">Координаты точки в клеточках</param>
        public PriceTimePoint GetPriceTimePosition(Point pointInCells)
            => new()
            {
                Price = CellPriceValues[pointInCells.Y],
                Date = CellTimeValues[pointInCells.X]
            };

        /// <summary>
        /// Перевод значения цены в клеточки
        /// </summary>
        public int PriceToCell(decimal price)
        {
            var delta = decimal.MaxValue;

            for (var i = 0; i < CellPriceValues.Length; i++)
            {
                var newDelta = Math.Abs(price - CellPriceValues[i]);

                if (newDelta <= delta)
                    delta = newDelta;
                else
                    return i;
            }

            return CellPriceValues.Length - 1;
        }
        
        /// <summary>
        /// Перевод значения времени в клеточки
        /// </summary>
        public int DateToCell(DateTime date)
        {
            var delta = int.MaxValue;

            for (var i = 0; i < CellTimeValues.Length; i++)
            {
                var newDelta =  DateWithTimeFrameHelpers.DatesDifferent(date, CellTimeValues[i], TimeFrameEnum.min1);

                if (newDelta <= delta)
                    delta = newDelta;
                else
                    return i;
            }

            return CellTimeValues.Length - 1;
        }
        
        internal void AddTimeValueWithSpread(int timeInCells, string description, ForecastCalculationResult result)
        {
            for (var spread = -ForecastSpread; spread <= ForecastSpread; spread++)
            {
                for (var y = ForecastWindow.Bottom; y <= ForecastWindow.Top; y++)
                {
                    result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(new Point(timeInCells + spread, y), description));
                }
            }
        }
        
        internal  void AddPriceValueWithSpread(int priceInCells, string description, ForecastCalculationResult result)
        {
            for (var spread = -ForecastSpread; spread <= ForecastSpread; spread++)
            {
                for (var x = ForecastWindow.Left; x <= ForecastWindow.Right; x++)
                {
                    result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(new Point(x, priceInCells + spread), description));
                }
            }
        }

        internal TimeRange GetForecastTimeRange()
        {
            var rangeLeft = TimeConverter.GetTimeSpan(Math.Max(ForecastWindow.Left - TargetPoint.X, 5), TimeFrame);
            var rangeRight = TimeConverter.GetTimeSpan(Math.Max(ForecastWindow.Right - TargetPoint.X, 5), TimeFrame);
            return new TimeRange(rangeLeft,rangeRight);
        }
    }
}