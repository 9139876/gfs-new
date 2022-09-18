using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models
{
    /// <summary> Точка тенденции </summary>
    public class TendentionPoint
    {
        /// <summary> Позиция в координатах цена время </summary>
        public PriceTimePoint Point { get; set; }

        /// <summary> Тип точки тенденции </summary>
        public TendentionPointTypeEnum TendentionPointType { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TendentionPoint tp
                   && tp.Point.Equals(Point)
                   && tp.TendentionPointType == TendentionPointType;
        }

        public static bool operator ==(TendentionPoint left, TendentionPoint right)
        {
            return (left is null && right is null) || (left is object && left.Equals(right));
        }

        public static bool operator !=(TendentionPoint left, TendentionPoint right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Point, TendentionPointType);
        }

        public override string ToString()
        {
            return $"{Description.GetDescription(TendentionPointType)} Дата:{Point.Date} Цена:{Point.Price}";
        }

        /// <summary>
        /// Получение строки настраиваемого формата
        /// </summary>
        /// <param name="time">Отображение времени</param>
        /// <param name="fractionalPart">Число знаков после запятой</param>
        /// <returns></returns>
        public string GetString(bool time, int fractionalPart)
        {
            var date = time ? Point.Date.ToString("g") : Point.Date.ToString("d");

            var price = Point.Price.ToString($"F{fractionalPart}");

            return $"{Description.GetDescription(TendentionPointType)} {date} - {price}";
        }
    }
}