using HistrixAPI.Enums;

namespace HistrixAPI.Models.Entities
{
    public class Position : IEntity
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
        public Side Side { get; set; }

        public int BotId { get; set; }
        public Bot Bot { get; set; }
    }
}
