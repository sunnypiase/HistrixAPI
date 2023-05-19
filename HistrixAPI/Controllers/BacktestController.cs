using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HistrixAPI.Backtesting;
using HistrixAPI.Repository.Abstract;
using HistrixAPI.Models.Entities;

namespace HistrixAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BacktestController : ControllerBase
    {
        private readonly BacktestModule _backtestModule;

        public BacktestController(
            IGenericRepository<Bot> botRepository,
            IGenericRepository<CandleEntity> candleRepository,
            IGenericRepository<Strategy> strategyRepository)
        {
            _backtestModule = new BacktestModule(botRepository, candleRepository, strategyRepository);
        }

        [HttpGet("{botId}")]
        public async Task<ActionResult<BacktestResult>> GetBacktestResult(int botId)
        {
            try
            {
                var result = await _backtestModule.RunBacktest(botId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
    }
}
