using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace GFS.GrailCommon.Models
{
    /// <summary> Точка тенденции </summary>
    public class TendentionPoint
    {
        /// <summary> Позиция в координатах цена время </summary>
        public PriceTimePoint Point { get; }

        /// <summary> Тип точки тенденции </summary>
        public TendentionPointTypeEnum TendentionPointType { get; private set; }

        public TendentionPoint(PriceTimePoint point)
        {
            Point = point;
            TendentionPointType = TendentionPointTypeEnum.Unknown;
        }

        public void SetPointType(TendentionPointTypeEnum pointType)
            => TendentionPointType = pointType;

        public override bool Equals(object? obj)
        {
            return obj is TendentionPoint tp
                   && tp.Point.Equals(Point)
                   && tp.TendentionPointType == TendentionPointType;
        }

        public static bool operator ==(TendentionPoint? left, TendentionPoint? right)
        {
            return left is not null && right is not null && left.Equals(right);
        }

        public static bool operator !=(TendentionPoint? left, TendentionPoint right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Point);
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

    public class TendentionPointComparer : IComparer<TendentionPoint>
    {
        public int Compare(TendentionPoint? x, TendentionPoint? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;

            return x.Point.Date.CompareTo(y.Point.Date);
        }
    }
}