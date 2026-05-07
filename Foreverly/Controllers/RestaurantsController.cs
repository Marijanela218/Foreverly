using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            return await _context.Restaurants
                .Include(r => r.Halls)
                .Include(r => r.Menus)
                .ToListAsync();
        }
    }
}