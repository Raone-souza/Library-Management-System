using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/Registro Açoes")]
    public class RegistroAcoesController : ControllerBase
    {
        private readonly LmsContext _context;

        public RegistroAcoesController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarAcao")]
        public IActionResult RegistrarAcao([FromBody] RegistroAcoes novoRegistro)
        {
            if (novoRegistro == null || novoRegistro.UsuarioID <= 0 || string.IsNullOrWhiteSpace(novoRegistro.TipoAcao))
                return BadRequest("Dados inválidos para registrar a ação.");

            try
            {
                novoRegistro.DataHora = DateTime.UtcNow;
                _context.RegistrosAcao.Add(novoRegistro);
                _context.SaveChanges();
                return CreatedAtAction(nameof(ObterRegistroPorId), new { id = novoRegistro.Id }, novoRegistro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("BuscarRegistroPorId/{id}")]
        public IActionResult ObterRegistroPorId(int id)
        {
            var registro = _context.RegistrosAcao.FirstOrDefault(r => r.Id == id);
            if (registro == null)
                return NotFound("Registro não encontrado.");

            return Ok(registro);
        }

        [HttpGet("ListarTodosRegistros")]
        public IActionResult ListarTodosRegistros()
        {
            var registros = _context.RegistrosAcao.ToList();
            if (!registros.Any())
                return NotFound("Nenhum registro de ação encontrado.");

            return Ok(registros);
        }

        [HttpDelete("DeletarRegistroPorId/{id}")]
        public IActionResult DeletarRegistroPorId(int id)
        {
            var registro = _context.RegistrosAcao.Find(id);
            if (registro == null)
                return NotFound("Registro não encontrado.");

            _context.RegistrosAcao.Remove(registro);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
