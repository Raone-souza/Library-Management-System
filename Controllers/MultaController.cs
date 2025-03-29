using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/Multas")]
    public class MultaController : ControllerBase
    {
        private readonly LmsContext _context;

        public MultaController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CriarMulta")]
        public IActionResult CriarMulta([FromBody] Multa novaMulta)
        {
            if (novaMulta == null || novaMulta.EmprestimoID <= 0 || novaMulta.LeitorID <= 0)
                return BadRequest("Dados inválidos para criar a multa.");

            try
            {
                var emprestimo = _context.Emprestimos.FirstOrDefault(e => e.Id == novaMulta.EmprestimoID);
                if (emprestimo == null)
                    return NotFound("Empréstimo não encontrado.");

                var leitor = _context.Leitores.FirstOrDefault(l => l.Id == novaMulta.LeitorID);
                if (leitor == null)
                    return NotFound("Leitor não encontrado.");

                novaMulta.DataGeracao = DateTime.UtcNow;
                novaMulta.Pago = false;

                _context.Multas.Add(novaMulta);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterMultaPorId), new { id = novaMulta.Id }, novaMulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("BuscarMultaPorId/{id}")]
        public IActionResult ObterMultaPorId(int id)
        {
            var multa = _context.Multas
                                .Include(m => m.Leitor)
                                .Include(m => m.Emprestimo)
                                .FirstOrDefault(m => m.Id == id);

            if (multa == null)
                return NotFound("Multa não encontrada.");

            return Ok(multa);
        }

        [HttpGet("ListarTodasMultas")]
        public IActionResult ListarTodasMultas()
        {
            var multas = _context.Multas
                                 .Include(m => m.Leitor)
                                 .Include(m => m.Emprestimo)
                                 .ToList();

            if (!multas.Any())
                return NotFound("Nenhuma multa encontrada.");

            return Ok(multas);
        }

        [HttpPut("AtualizarMultaPorId/{id}")]
        public IActionResult AtualizarMultaPorId(int id, [FromBody] Multa multaAtualizada)
        {
            var multa = _context.Multas.Find(id);
            if (multa == null)
                return NotFound("Multa não encontrada.");

            multa.Valor = multaAtualizada.Valor;
            multa.DataGeracao = multaAtualizada.DataGeracao;
            multa.Pago = multaAtualizada.Pago;

            _context.Multas.Update(multa);
            _context.SaveChanges();

            return Ok(multa);
        }

        [HttpPut("PagarMultaPorId/{id}")]
        public IActionResult PagarMultaPorId(int id)
        {
            var multa = _context.Multas.Find(id);
            if (multa == null)
                return NotFound("Multa não encontrada.");

            if (multa.Pago)
                return BadRequest("Esta multa já foi paga.");

            multa.Pago = true;
            _context.Multas.Update(multa);
            _context.SaveChanges();

            return Ok(multa);
        }

        [HttpDelete("DeletarMultaPorId/{id}")]
        public IActionResult DeletarMultaPorId(int id)
        {
            var multa = _context.Multas.Find(id);
            if (multa == null)
                return NotFound("Multa não encontrada.");

            _context.Multas.Remove(multa);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
