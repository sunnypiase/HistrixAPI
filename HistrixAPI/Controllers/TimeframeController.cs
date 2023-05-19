using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HistrixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeframeController : ControllerBase
    {
        private readonly IGenericRepository<Timeframe> _repository;

        public TimeframeController(IGenericRepository<Timeframe> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timeframe>>> GetAll()
        {
            return Ok(await _repository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Timeframe>> GetById(int id)
        {
            var timeframe = await _repository.GetByIdAsync(id);
            if (timeframe == null)
            {
                return NotFound();
            }
            return Ok(timeframe);
        }

        [HttpPost]
        public async Task<ActionResult<Timeframe>> Create(Timeframe timeframe)
        {
            var created = await _repository.InsertAsync(timeframe);
            if (created)
            {
                return CreatedAtAction(nameof(GetById), new { id = timeframe.Id }, timeframe);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Timeframe timeframe)
        {
            if (id != timeframe.Id)
            {
                return BadRequest();
            }

            var updated = await _repository.UpdateAsync(timeframe);

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

}
