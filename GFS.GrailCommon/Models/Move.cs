namespace GFS.GrailCommon.Models
{
    /// <summary> Движение </summary>
    public class Move
    {
        public Move(TendentionPoint begin, TendentionPoint end)
        {
            if (end.Point.Date <= begin.Point.Date)
                throw new InvalidOperationException(
                    $"Дата конечной точки {end.Point.Date} должна быть больше даты начальной точки {begin.Point.Date}");

            Begin = begin;
            End = end;
        }

        /// <summary> Начало </summary>
        public TendentionPoint Begin { get; }

        /// <summary> Конец </summary>
        public TendentionPoint End { get; }

        /// <summary> Величина движения по цене </summary>
        public decimal PriceMove() => End.Point.Price - Begin.Point.Price;

        /// <summary> Величина движения по времени </summary>
        public TimeSpan TimeMove() => End.Point.Date - Begin.Point.Date;
    }
}