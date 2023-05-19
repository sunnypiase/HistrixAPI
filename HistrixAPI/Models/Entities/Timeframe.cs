using HistrixAPI.Enums;

namespace HistrixAPI.Models.Entities
{
    public class Timeframe : IEntity
    {
        public int Id { get; set; }
        public TimeframeDuration TimeframeDuration { get; set; }
        public ICollection<CandleEntity> Candles { get; set; }
    }
}
