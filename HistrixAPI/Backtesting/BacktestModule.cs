using HistrixAPI.Enums;
using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;

namespace HistrixAPI.Backtesting
{
    public class BacktestModule
    {
        private readonly IGenericRepository<Bot> _botRepository;
        private readonly IGenericRepository<CandleEntity> _candleRepository;
        private readonly IGenericRepository<Strategy> _strategyRepository;

        public BacktestModule(
            IGenericRepository<Bot> botRepository,
            IGenericRepository<CandleEntity> candleRepository,
            IGenericRepository<Strategy> strategyRepository)
        {
            _botRepository = botRepository;
            _candleRepository = candleRepository;
            _strategyRepository = strategyRepository;
        }

        public async Task<BacktestResult> RunBacktest(int botId)
        {
            var bot = await _botRepository.GetByIdAsync(botId);
            if (bot == null)
            {
                throw new Exception("Bot not found");
            }

            var strategy = await _strategyRepository.GetByIdAsync(bot.StrategyId);
            var candles = await _candleRepository.GetAsync(c => c.CryptoPairId == bot.CryptoPairId);

            IBacktestStrategy? backtestStrategy = default;
            backtestStrategy = strategy.StrategyName switch
            {
                StrategyName.Grid => new GridBacktestStrategy((GridStrategy)strategy),
                _ => throw new Exception("Invalid strategy type"),
            };
            var result = backtestStrategy.Run(candles, 10000);
            return result;
        }
    }

    public interface IBacktestStrategy
    {
        BacktestResult Run(IEnumerable<CandleEntity> candles, double startingCapital);
    }

    public class GridBacktestStrategy : IBacktestStrategy
    {
        private readonly GridStrategy _strategy;
        private readonly Queue<double> _buyLevels;
        private readonly Queue<double> _sellLevels;
        private readonly Queue<Position> _openPositions;

        public GridBacktestStrategy(GridStrategy strategy)
        {
            _strategy = strategy;
            _buyLevels = new Queue<double>();
            _sellLevels = new Queue<double>();
            _openPositions = new Queue<Position>();
        }

        public BacktestResult Run(IEnumerable<CandleEntity> candles, double startingCapital)
        {
            int dealCount = 0;
            double totalProfit = 0;
            double currentCapital = startingCapital;
            double orderSize = startingCapital / _strategy.LevelsCount; // Size of each order

            foreach (var candle in candles)
            {
                // If the closing price is lower than the next buy level and we have sufficient capital
                while (_buyLevels.Any() && candle.Close < _buyLevels.Peek() && currentCapital >= orderSize)
                {
                    // Buy at this level.
                    currentCapital -= orderSize;
                    _openPositions.Enqueue(new Position { Price = _buyLevels.Dequeue(), Side = Side.Buy });
                }

                // If the closing price is higher than the next sell level and we have open positions
                while (_sellLevels.Any() && candle.Close > _sellLevels.Peek() && _openPositions.Any())
                {
                    // Sell at this level.
                    var position = _openPositions.Dequeue();
                    double profit = candle.Close - position.Price; // Profit should be calculated with the closing price
                    totalProfit += profit;
                    currentCapital += orderSize + profit; // Add both the order size and profit to the current capital
                    _sellLevels.Dequeue();
                    dealCount++;
                }

                // Generate new grid levels if we have enough capital for a new grid and the grid isn't full yet.
                if ((!_buyLevels.Any() || !_sellLevels.Any()) && _openPositions.Count < _strategy.LevelsCount && currentCapital >= orderSize * _strategy.LevelsCount)
                {
                    for (int i = 1; i <= _strategy.LevelsCount; i++)
                    {
                        _buyLevels.Enqueue(candle.Close - _strategy.LevelsDistance * i);
                        _sellLevels.Enqueue(candle.Close + _strategy.LevelsDistance * i);
                    }
                }
            }

            // Sell remaining positions at the last closing price.
            while (_openPositions.Any())
            {
                var position = _openPositions.Dequeue();
                double profit = candles.Last().Close - position.Price;
                totalProfit += profit;
                currentCapital += orderSize + profit; // Add both the order size and profit to the current capital
                dealCount++;
            }

            return new BacktestResult { Profit = totalProfit, DealCount = dealCount, FinalCapital = currentCapital };
        }
    }





    public class BacktestResult
    {
        public double Profit { get; set; }
        public double FinalCapital { get; set; }
        public int DealCount { get; set; }
        // Add other metrics you are interested in...
    }

}
