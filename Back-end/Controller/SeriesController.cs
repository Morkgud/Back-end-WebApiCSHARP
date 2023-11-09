﻿using Microsoft.AspNetCore.Mvc;
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
    public class SeriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Serie>>> GetSeries()
        {
            return await _context.Series.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Serie>> GetSerie(int id)
        {
            var serie = await _context.Series.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        [HttpPost]
        public async Task<ActionResult<Serie>> PostSerie(Serie serie)
        {
            _context.Series.Add(serie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSerie", new { id = serie.Id }, serie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSerie(int id, Serie serie)
        {
            if (id != serie.Id)
            {
                return BadRequest();
            }

            _context.Entry(serie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(id))
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
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchSerie(int idSerie, [FromBody] Serie serieAtualizada)
        {
            try
            {
                var serieExistente = await _context.Series.FindAsync(idSerie);

                if (serieExistente == null)
                    return NotFound();

                // Atualize apenas as propriedades que foram fornecidas na serieAtualizada
                if (serieAtualizada.Nome != null)
                    serieExistente.Nome = serieAtualizada.Nome;

                if (serieAtualizada.Ano != 0)
                    serieExistente.Ano = serieAtualizada.Ano;

                if (serieAtualizada.Temporadas != 0)
                    serieExistente.Temporadas = serieAtualizada.Temporadas;

                if (serieAtualizada.Episodios != 0)
                    serieExistente.Episodios = serieAtualizada.Episodios;

                if (serieAtualizada.Finalizada != serieExistente.Finalizada)
                    serieExistente.Finalizada = serieAtualizada.Finalizada;

                if (serieAtualizada.Genero != null)
                    serieExistente.Genero = serieAtualizada.Genero;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            var serie = await _context.Series.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            _context.Series.Remove(serie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SerieExists(int id)
        {
            return _context.Series.Any(e => e.Id == id);
        }
    }
}
