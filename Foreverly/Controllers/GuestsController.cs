using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuestsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("wedding/{weddingId}")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests(int weddingId)
        {
            return await _context.Guests
                .Where(g => g.WeddingId == weddingId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Guest>> AddGuest(Guest guest)
        {
            _context.Guests.Add(guest);

            await _context.SaveChangesAsync();

            return Ok(guest);
        }
    }
}