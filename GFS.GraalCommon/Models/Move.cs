namespace GFS.GraalCommon.Models
{
    /// <summary>
    /// Движение
    /// </summary>
    public class Move
    {
        public Move(TendentionPoint begin, TendentionPoint end)
        {
            if (end.Date <= begin.Date)
                throw new InvalidOperationException($"Дата конечной точки {end.Date} должна быть больше даты начальной точки {begin.Date}");

            Begin = begin;
            End = end;
        }

        /// <summary>
        /// Начало
        /// </summary>
        public TendentionPoint Begin { get; private set; }

        /// <summary>
        /// Конец
        /// </summary>
        public TendentionPoint End { get; private set; }

        /// <summary>
        /// Величина движения по цене
        /// </summary>
        /// <returns></returns>
        public decimal PriceMove() => End.Price - Begin.Price;

        /// <summary>
        /// Величина движения по времени
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimeMove() => End.Date - Begin.Date;
    }
}
