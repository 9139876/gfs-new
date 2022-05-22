using GFS.GraalCommon.Enums;

namespace GFS.GraalCommon.Models
{
    /// <summary>
    /// Точка тенденции
    /// </summary>
    public class TendentionPoint
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Тип точки тенденции
        /// </summary>
        public TendentionPointTypeEnum TendentionPointType { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TendentionPoint tp
                && tp.Date == Date
                && tp.Price == Price
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
            return HashCode.Combine(Date, Price, TendentionPointType);
        }

        public override string ToString()
        {
            return $"{(TendentionPointType == TendentionPointTypeEnum.Top ? "Вершина" : "Основание")} Дата:{Date} Цена:{Price}";
        }

        /// <summary>
        /// Получение строки настраиваемого формата
        /// </summary>
        /// <param name="time">Отображение времени</param>
        /// <param name="fractionalPart">Число знаков после запятой</param>
        /// <returns></returns>
        public string GetString(bool time, int fractionalPart)
        {
            var date = time ? Date.ToString("g") : Date.ToString("d");

            var price = Price.ToString($"F{fractionalPart}");

            return $"{(TendentionPointType == TendentionPointTypeEnum.Top ? "Вершина" : "Основание")} {date} - {price}";
        }
    }
}
