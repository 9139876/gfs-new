namespace GFS.Portfolio.Api.Enums
{
    public enum OperationTypeEnum
    {
        /// <summary>
        /// Забрать деньги
        /// </summary>
        Take = 1,

        /// <summary>
        /// Внести деньги
        /// </summary>
        Deposit = 2,

        /// <summary>
        /// Купить актив или закрыть по нему короткую позицию
        /// </summary>
        Buy = 3,

        /// <summary>
        /// Продать актив или закрыть по нему короткую позицию
        /// </summary>
        Sell = 4
    }
}