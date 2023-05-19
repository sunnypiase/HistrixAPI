using HistrixAPI.Enums;
using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HistrixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StrategyController : ControllerBase
    {
        private readonly IGenericRepository<Strategy> _repository;

        public StrategyController(IGenericRepository<Strategy> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Strategy>>> GetAll()
        {
            return Ok(await _repository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Strategy>> GetById(int id)
        {
            var strategy = await _repository.GetByIdAsync(id);
            if (strategy == null)
            {
                return NotFound();
            }
            return Ok(strategy);
        }

        [HttpPost]
        public async Task<ActionResult<Strategy>> Create(CreateStrategyDto createStrategyDto)
        {
            Strategy strategy;

            switch (createStrategyDto.Name)
            {
                case StrategyName.Grid:
                    strategy = JsonSerializer.Deserialize<GridStrategy>(createStrategyDto.Json);
                    break;
                case StrategyName.SMA:
                    strategy = JsonSerializer.Deserialize<SMAStrategy>(createStrategyDto.Json);
                    break;
                default:
                    return BadRequest("Invalid strategy name");
            }

            if (strategy == null)
            {
                return BadRequest("Invalid strategy data");
            }

            var created = await _repository.InsertAsync(strategy);
            if (created)
            {
                return CreatedAtAction(nameof(GetById), new { id = strategy.Id }, strategy);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Strategy strategy)
        {
            if (id != strategy.Id)
            {
                return BadRequest();
            }

            var updated = await _repository.UpdateAsync(strategy);

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
    public class CreateStrategyDto
    {
        public StrategyName Name { get; set; }
        public string Json { get; set; }
    }

}
