using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TemplatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates
                .Include(t => t.TemplateItems)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Template>> CreateTemplate(Template template)
        {
            _context.Templates.Add(template);

            await _context.SaveChangesAsync();

            return Ok(template);
        }
    }
}