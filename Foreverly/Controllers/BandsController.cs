using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Band>>> GetBands()
        {
            return await _context.Bands
                .Include(b => b.Playlists)
                .Include(b => b.BandPrices)
                .ToListAsync();
        }
    }
}