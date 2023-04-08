namespace GFS.Broker.Api.Models.Portfolio
{
    public class AssetModel
    {
        public Guid AssetId { get; set; }

        /// <summary>
        /// Может быть положительным(длинная позиция) или отрицательным (короткая позиция) числом 
        /// </summary>
        public int Count { get; set; }
    }
}