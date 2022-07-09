using System;

namespace GFS.GrailCommon.Models
{
    public class QuoteModel
    {
        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal Hi { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }
}