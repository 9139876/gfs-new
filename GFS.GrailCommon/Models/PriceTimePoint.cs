namespace GFS.GrailCommon.Models
{
    /// <summary> Позиция в координатах цена время </summary>
    public class PriceTimePoint
    {
        /// <summary> Дата </summary>
        public DateTime Date { get; init; }

        /// <summary> Цена </summary>
        public decimal Price { get; init; }

        public override bool Equals(object? obj)
            =>  obj is PriceTimePoint point 
                && point.Date.Equals(Date) 
                && point.Price.Equals(Price);

        public override int GetHashCode()
            => HashCode.Combine(Date, Price);
    }
}