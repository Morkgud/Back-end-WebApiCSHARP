using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiFilmesSeries.Data;
using WebApiFilmesSeries.Models;

namespace WebApiFilmesSeries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EsportesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EsportesController(AppDbContext context)
        {
            _context = context;
        }

        // Get esportes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Esportes>>> GetEsportes()
        {
            return await _context.Esportes.ToListAsync();
        }

        // GET: api/Esportes
        [HttpGet("{id}")]
        public async Task<ActionResult<Esportes>> GetEsporte(int id)
        {
            var esporte = await _context.Esportes.FindAsync(id);
            if (esporte == null)
            {
                return NotFound();
            }

            return esporte;
        }

        // POST: api/Esportes
        [HttpPost]
        public async Task<ActionResult<Esportes>> CreateEsporte([FromBody] Esportes esporte)
        {
            _context.Esportes.Add(esporte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEsporte", new { id = esporte.Id }, esporte);
        }

        // PUT: api/Esportes
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEsporte(int id, [FromBody] Esportes esporte)
        {
            if (id != esporte.Id)
            {
                return BadRequest();
            }

            _context.Entry(esporte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EsporteExists(id))
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

        // PATCH: api/Esportes
        [HttpPatch("{idEsporte}")]
        public async Task<ActionResult> PatchEsporte(int idEsporte, [FromBody] Esportes esporteAtualizado)
        {
            try
            {
                var esporteExistente = await _context.Esportes.FindAsync(idEsporte);

                if (esporteExistente == null)
                    return NotFound();

                // Atualize apenas as propriedades que foram fornecidas no esporteAtualizado
                if (esporteAtualizado.NomeEsporte != null)
                    esporteExistente.NomeEsporte = esporteAtualizado.NomeEsporte;

                if (esporteAtualizado.Campeonato != null)
                    esporteExistente.Campeonato = esporteAtualizado.Campeonato;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Delete para um esporte
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEsporte(int id)
        {
            var esporte = await _context.Esportes.FindAsync(id);

            if (esporte == null)
            {
                return NotFound();
            }

            _context.Esportes.Remove(esporte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EsporteExists(int id)
        {
            return _context.Esportes.Any(e => e.Id == id);
        }
    }
}
