using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DASI_2025A.Backend.Data;
using DASI_2025A.Backend.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CombosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Combos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Combos>>> GetCombos()
        {
            return await _context.Combos.ToListAsync();
        }

        // GET: api/Combos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Combos>> GetCombos(int id)
        {
            var combo = await _context.Combos.FindAsync(id);

            if (combo == null)
            {
                return NotFound();
            }

            return combo;
        }

        // POST: api/Combos
        [HttpPost]
        public async Task<ActionResult<Combos>> PostCombos(Combos combos)
        {
            _context.Combos.Add(combos);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCombos), new { id = combos.Id }, combos);
        }

        // PUT: api/Combos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCombos(int id, Combos combos)
        {
            if (id != combos.Id)
            {
                return BadRequest();
            }

            _context.Entry(combos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CombosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Combos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombos(int id)
        {
            var combos = await _context.Combos.FindAsync(id);
            if (combos == null)
            {
                return NotFound();
            }

            _context.Combos.Remove(combos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CombosExists(int id)
        {
            return _context.Combos.Any(e => e.Id == id);
        }
    }
}
