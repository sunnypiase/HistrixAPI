using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HistrixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IGenericRepository<Bot> _repository;
        private readonly IGenericRepository<Strategy> _strategyRepository;
        private readonly IGenericRepository<CryptoPair> _cryptoPairRepository;
        private readonly IGenericRepository<Timeframe> _timeframeRepository;
        private readonly IGenericRepository<User> _userRepository;

        public BotController(IGenericRepository<Bot> repository,
            IGenericRepository<Strategy> strategyRepository,
            IGenericRepository<CryptoPair> cryptoPairRepository,
            IGenericRepository<Timeframe> timeframeRepository,
            IGenericRepository<User> userRepository)
        {
            _repository = repository;
            _strategyRepository = strategyRepository;
            _cryptoPairRepository = cryptoPairRepository;
            _timeframeRepository = timeframeRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bot>>> GetAll()
        {
            return Ok(await _repository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bot>> GetById(int id)
        {
            var bot = await _repository.GetByIdAsync(id);
            if (bot == null)
            {
                return NotFound();
            }
            return Ok(bot);
        }

        [HttpPost]
        public async Task<ActionResult<Bot>> Create(CreateBotDto createBotDto)
        {
            var strategy = await _strategyRepository.GetByIdAsync(createBotDto.StrategyId);
            if (strategy == null)
            {
                return NotFound("Strategy not found");
            }

            var cryptoPair = await _cryptoPairRepository.GetByIdAsync(createBotDto.CryptoPairId);
            if (cryptoPair == null)
            {
                return NotFound("CryptoPair not found");
            }

            var timeframe = await _timeframeRepository.GetByIdAsync(createBotDto.TimeframeId);
            if (timeframe == null)
            {
                return NotFound("Timeframe not found");
            }

            User? user = null;
            if (createBotDto.UserId.HasValue)
            {
                user = await _userRepository.GetByIdAsync(createBotDto.UserId.Value);
                if (user == null)
                {
                    return NotFound("User not found");
                }
            }

            var bot = new Bot
            {
                CryptoPair = cryptoPair,
                Strategy = strategy,
                Timeframe = timeframe,
                User = user,
            };

            var created = await _repository.InsertAsync(bot);
            if (created)
            {
                return CreatedAtAction(nameof(GetById), new { id = bot.Id }, bot);
            }

            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Bot bot)
        {
            if (id != bot.Id)
            {
                return BadRequest();
            }

            var updated = await _repository.UpdateAsync(bot);

            if (updated)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
    public class CreateBotDto
    {
        public int CryptoPairId { get; set; }
        public int TimeframeId { get; set; }
        public int StrategyId { get; set; }
        public int? UserId { get; set; }
    }
}
