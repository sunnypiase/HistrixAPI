namespace HistrixAPI.Models.Entities
{
    public class CandleEntity: IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }

        public int CryptoPairId { get; set; }

        public CryptoPair CryptoPair { get; set; }

        public int TimeframeId { get; set; }

        public Timeframe Timeframe { get; set; }
    }
}
