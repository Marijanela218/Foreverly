using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partner>>> GetPartners()
        {
            return await _context.Partners
                .Include(p => p.Category)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Partner>> CreatePartner(Partner partner)
        {
            _context.Partners.Add(partner);

            await _context.SaveChangesAsync();

            return Ok(partner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartner(int id)
        {
            var partner = await _context.Partners.FindAsync(id);

            if (partner == null)
                return NotFound();

            _context.Partners.Remove(partner);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}