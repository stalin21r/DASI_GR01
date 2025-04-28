using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Data.Models;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public CombosController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: api/Combos
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Combos>>>> GetCombos() {
            var combos = await _context.Combos.ToListAsync();
            return Ok(new ApiResponse<IEnumerable<Combos>>(200, "Combos obtenidos con éxito", combos));
        }

        // GET: api/Combos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Combos>>> GetCombo(int id) {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null) {
                return NotFound(new ApiResponse<Combos>(404, "Combo no encontrado"));
            }

            return Ok(new ApiResponse<Combos>(200, "Combo obtenido con éxito", combo));
        }

        // POST: api/Combos
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> PostCombo(Combos combo) {
            try {
                _context.Combos.Add(combo);
                await _context.SaveChangesAsync();

                return StatusCode(201, new ApiResponse<object>(201, "Combo creado con éxito"));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(500, $"Error al crear combo: {ex.Message}"));
            }
        }

        // PUT: api/Combos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> PutCombo(int id, Combos combo) {
            if (id != combo.Id) {
                return BadRequest(new ApiResponse<object>(400, "El ID del combo no coincide"));
            }

            _context.Entry(combo).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                return Ok(new ApiResponse<object>(200, "Combo guardado con éxito"));
            }
            catch (DbUpdateConcurrencyException) {
                if (!CombosExists(id)) {
                    return NotFound(new ApiResponse<object>(404, "Combo no encontrado"));
                }
                else {
                    return StatusCode(500, new ApiResponse<object>(500, "Error de concurrencia al guardar combo"));
                }
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(500, $"Error al guardar combo: {ex.Message}"));
            }
        }

        // DELETE: api/Combos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCombo(int id) {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null) {
                return NotFound(new ApiResponse<object>(404, "Combo no encontrado"));
            }

            try {
                _context.Combos.Remove(combo);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>(200, "Combo eliminado con éxito"));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(500, $"Error al eliminar combo: {ex.Message}"));
            }
        }

        private bool CombosExists(int id) {
            return _context.Combos.Any(e => e.Id == id);

        }
    }
}
