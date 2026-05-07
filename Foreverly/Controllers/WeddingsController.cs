using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeddingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeddingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wedding>>> GetWeddings()
        {
            return await _context.Weddings
                .Include(w => w.Template)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wedding>> GetWedding(int id)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Guests)
                .Include(w => w.WeddingServices)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wedding == null)
                return NotFound();

            return wedding;
        }

        [HttpPost]
        public async Task<ActionResult<Wedding>> CreateWedding(Wedding wedding)
        {
            _context.Weddings.Add(wedding);
            await _context.SaveChangesAsync();

            return Ok(wedding);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWedding(int id, Wedding wedding)
        {
            if (id != wedding.Id)
                return BadRequest();

            _context.Entry(wedding).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWedding(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);

            if (wedding == null)
                return NotFound();

            _context.Weddings.Remove(wedding);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}