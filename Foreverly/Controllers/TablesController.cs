using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TablesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("wedding/{weddingId}")]
        public async Task<ActionResult<IEnumerable<WeddingTable>>> GetTables(int weddingId)
        {
            return await _context.WeddingTables
                .Where(t => t.WeddingId == weddingId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WeddingTable>> CreateTable(WeddingTable table)
        {
            _context.WeddingTables.Add(table);

            await _context.SaveChangesAsync();

            return Ok(table);
        }
    }
}