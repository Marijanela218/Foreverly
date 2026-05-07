using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeddingServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeddingServicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeddingService>>> GetServices()
        {
            return await _context.WeddingServices
                .Include(ws => ws.Partner)
                .Include(ws => ws.Wedding)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WeddingService>> AddService(WeddingService service)
        {
            service.TotalPrice = service.Quantity * service.UnitPrice;

            service.CommissionAmount =
                service.TotalPrice * (service.CommissionPercent / 100);

            _context.WeddingServices.Add(service);

            await _context.SaveChangesAsync();

            return Ok(service);
        }
    }
}