using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HistrixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoPairController : ControllerBase
    {
        private readonly IGenericRepository<CryptoPair> _repository;

        public CryptoPairController(IGenericRepository<CryptoPair> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CryptoPair>>> GetAll()
        {
            return Ok(await _repository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CryptoPair>> GetById(int id)
        {
            var cryptoPair = await _repository.GetByIdAsync(id);
            if (cryptoPair == null)
            {
                return NotFound();
            }
            return Ok(cryptoPair);
        }

        [HttpPost]
        public async Task<ActionResult<CryptoPair>> Create(CryptoPair cryptoPair)
        {
            var created = await _repository.InsertAsync(cryptoPair);
            if (created)
            {
                return CreatedAtAction(nameof(GetById), new { id = cryptoPair.Id }, cryptoPair);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CryptoPair cryptoPair)
        {
            if (id != cryptoPair.Id)
            {
                return BadRequest();
            }

            var updated = await _repository.UpdateAsync(cryptoPair);

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
