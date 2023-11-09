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
    public class FilmesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Filmes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
        {
            var filmes = await _context.Filmes.ToListAsync();
            if (filmes.Any())
            {
                return filmes;
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Filmes
        [HttpGet("{id}")]
        public async Task<ActionResult<Filme>> GetFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            return filme;
        }

        // POST: api/Filmes
        [HttpPost]
        public async Task<ActionResult<Filme>> PostFilme(Filme filme)
        {
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilme", new { id = filme.Id }, filme);
        }

        // PUT: api/Filmes
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilme(int id, Filme filme)
        {
            if (id != filme.Id)
            {
                return BadRequest();
            }

            _context.Entry(filme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmeExists(id))
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

        // PATCH: api/Filmes
        [HttpPatch("{idFilme}")]
        public async Task<ActionResult> PatchFilme(int idFilme, [FromBody] Filme filmeAtualizado)
        {
            try
            {
                var filmeExistente = await _context.Filmes.FindAsync(idFilme);

                if (filmeExistente == null)
                {
                    return NotFound();
                }

                // Atualize apenas as propriedades que foram fornecidas no filmeAtualizado
                if (filmeAtualizado.Nome != null)
                {
                    filmeExistente.Nome = filmeAtualizado.Nome;
                }

                if (filmeAtualizado.Ano != 0)
                {
                    filmeExistente.Ano = filmeAtualizado.Ano;
                }

                if (filmeAtualizado.Diretor != null)
                {
                    filmeExistente.Diretor = filmeAtualizado.Diretor;
                }

                if (filmeAtualizado.Duracao != 0)
                {
                    filmeExistente.Duracao = filmeAtualizado.Duracao;
                }

                if (filmeAtualizado.Genero != null)
                {
                    filmeExistente.Genero = filmeAtualizado.Genero;
                }

                if (filmeAtualizado.Estudio != null)
                {
                    filmeExistente.Estudio = filmeAtualizado.Estudio;
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.Id == id);
        }
    }
}
