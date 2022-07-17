using GFS.GrailCommon.Models;

namespace GFS.Portfolio.Api.Models
{
    public class AssetModel
    {
        public AssetIdentifier AssetIdentifier { get; set; }

        /// <summary>
        /// Может быть положительным(длинная позиция) или отрицательным (короткая позиция) числом 
        /// </summary>
        public int Count { get; set; }
    }
}