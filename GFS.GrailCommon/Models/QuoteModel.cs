using GFS.Common.Attributes.Validation;
using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models
{
    public class QuoteModel
    {
        public TimeFrameEnum TimeFrame { get; set; }
        
        [UtcDate]
        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal? Volume { get; set; }
    }
}