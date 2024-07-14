using System.Drawing;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
#pragma warning disable CS8618

namespace GFS.AnalysisSystem.Library.Calculation.Models
{
    public class CalculationContext
    {
        public CalculationContext(
            TimeFrameEnum timeFrame, 
            ForecastWindow forecastWindow, 
            Size sheetSizeInCells, 
            DateTime[] timeValues,
            decimal[] priceValues,
            byte forecastSpread,
            Point[] pointsFrom)
        {
            TimeFrame = timeFrame;
            ForecastWindow = forecastWindow;
            SheetSizeInCells = sheetSizeInCells;
            TimeValues = timeValues;
            PriceValues = priceValues;
            ForecastSpread = forecastSpread;
            PointsFrom = pointsFrom;
        }
        
        /// <summary>
        /// Таймфрейм листа
        /// </summary>
        public TimeFrameEnum TimeFrame{ get;  }
     
        /// <summary>
        /// Окно прогнозирования
        /// </summary>
        public ForecastWindow ForecastWindow { get;  }
        
        /// <summary>
        /// Размер листа в клеточках
        /// </summary>
        public Size SheetSizeInCells { get;  }

        /// <summary>
        /// Значения дат для клеточек
        /// </summary>
        public DateTime[] TimeValues { get;  }

        /// <summary>
        /// Значения цен для клеточек
        /// </summary>
        public decimal[] PriceValues { get;  }

        /// <summary>
        /// Разброс прогноза 
        /// </summary>
        public byte ForecastSpread { get;  }

        /// <summary>
        /// Коллекция точек,от которых делается прогноз 
        /// </summary>
        public Point[] PointsFrom { get;  }

        /// <summary>
        /// Проверяет, попадает ли точка в окно прогнозирования
        /// </summary>
        /// <param name="pointInCells">Координаты точки в клеточках</param>
        public bool InForecastWindow(Point pointInCells) 
            => ForecastWindow.InWindow(pointInCells);

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