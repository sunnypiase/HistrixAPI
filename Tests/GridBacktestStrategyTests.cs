using HistrixAPI.Models.Entities;

namespace HistrixAPI.Backtesting.Tests
{
    public class GridBacktestStrategyTests
    {
        [Fact]
        public async Task Run_ShouldIncreaseProfit_WhenPricesFallAndThenRise()
        {
            // Arrange
            var strategy = new GridStrategy
            {
                LevelsDistance = 1,
                LevelsCount = 10
            };
            var candles = GetFallingThenRisingCandles();
            var backtestStrategy = new GridBacktestStrategy(strategy);

            // Act
            var result = backtestStrategy.Run(candles, 10000);

            // Assert
            Assert.True(result.Profit > 0);
        }

        [Fact]
        public async Task Run_ShouldDecreaseCapital_WhenPricesAreFalling()
        {
            // Arrange
            var strategy = new GridStrategy
            {
                LevelsDistance = 1,
                LevelsCount = 10
            };
            var candles = GetFallingCandles();
            var backtestStrategy = new GridBacktestStrategy(strategy);

            // Act
            var result = backtestStrategy.Run(candles, 10000);

            // Assert
            Assert.True(result.FinalCapital < 10000);
        }

        private IEnumerable<CandleEntity> GetFallingThenRisingCandles()
        {
            return Enumerable.Range(1, 100).Select(i => new CandleEntity { Close = i <= 50 ? 100 - i : i });
        }

        private IEnumerable<CandleEntity> GetFallingCandles()
        {
            return Enumerable.Range(1, 100).Select(i => new CandleEntity { Close = 100 - i });
        }
    }
}
