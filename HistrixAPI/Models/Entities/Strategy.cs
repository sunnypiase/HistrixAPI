using HistrixAPI.Enums;

namespace HistrixAPI.Models.Entities
{
    public class Strategy : IEntity
    {
        public int Id { get; set; }
        public StrategyName StrategyName { get; set; }
    }

    public class GridStrategy : Strategy
    {
        public double LevelsDistance { get; set; }
        public int LevelsCount { get; set; }
    }

    public class SMAStrategy : Strategy
    {
        public int SlowSMA { get; set; }
        public int FastSMA { get; set; }
    }
}