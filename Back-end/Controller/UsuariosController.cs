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
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            if (usuarios.Any())
            {
                return usuarios;
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Usuario
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id || !UsuarioExists(id))
            {
                return BadRequest();
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound();
            }

            AtualizarPropriedadesUsuario(usuarioExistente, usuario);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PATCH: api/Usuario
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchUsuario(int id, [FromBody] Usuario usuarioAtualizado)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound();
            }

            AtualizarPropriedadesUsuario(usuarioExistente, usuarioAtualizado);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void AtualizarPropriedadesUsuario(Usuario usuarioExistente, Usuario usuarioAtualizado)
        {
            if (usuarioExistente != null && usuarioAtualizado != null)
            {
                usuarioExistente.Nome = usuarioAtualizado.Nome;
                // Adicione outras propriedades que você deseja atualizar
            }
        }
    }
}
