using HistrixAPI.Enums;

namespace HistrixAPI.Models.Entities
{
    public class CryptoPair: IEntity
    {
        public int Id { get; set; }
        public string CryptoPairName { get; set; }
        public ICollection<CandleEntity> Candles { get; set; }
    }
}
