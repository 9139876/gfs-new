using System;

namespace GFS.GrailCommon.Models
{
    /// <summary>
    /// Универсальный идентификатор инструмента
    /// </summary>
    public class AssetIdentifier
    {
        /// <summary>
        /// Имя рынка
        /// </summary>
        public string MarketName { get; set; }

        /// <summary>
        /// Имя инструмента
        /// </summary>
        public string TickerName { get; set; }

        public override bool Equals(object? obj) =>
            obj is AssetIdentifier tickerAndMarket
            && MarketName == tickerAndMarket.MarketName
            && TickerName == tickerAndMarket.TickerName;


        public override int GetHashCode() => HashCode.Combine(MarketName, TickerName);

        public static bool operator ==(AssetIdentifier? left, AssetIdentifier? right)
        {
            if (Equals(left, null))
                return (Equals(right, null));
            else if (Equals(right, null))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(AssetIdentifier? left, AssetIdentifier? right) => !(left == right);

        public override string ToString() => $"{TickerName} рынок {MarketName}";
    }
}