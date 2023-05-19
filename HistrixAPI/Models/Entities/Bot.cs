namespace HistrixAPI.Models.Entities
{
    public class Bot : IEntity
    {
        public int Id { get; set; }

        public int CryptoPairId { get; set; }
        public CryptoPair CryptoPair { get; set; }

        public int TimeframeId { get; set; }
        public Timeframe Timeframe { get; set; }

        public int StrategyId { get; set; }
        public Strategy Strategy { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
