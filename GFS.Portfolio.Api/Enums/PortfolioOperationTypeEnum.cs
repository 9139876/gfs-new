namespace GFS.Portfolio.Api.Enums
{
    public enum PortfolioOperationTypeEnum
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
        /// Купить инструмент или закрыть по нему короткую позицию
        /// </summary>
        Buy = 3,

        /// <summary>
        /// Продать инструмент или закрыть по нему длинную позицию
        /// </summary>
        Sell = 4
    }
}