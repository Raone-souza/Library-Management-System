using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/Emprestimos")]
    public class EmprestimoController : ControllerBase
    {
        private readonly LmsContext _context;

        public EmprestimoController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CriarEmprestimo")]
        public IActionResult CriarEmprestimo([FromBody] Emprestimo novoEmprestimo)
        {
            if (novoEmprestimo == null || novoEmprestimo.LeitorID <= 0 || novoEmprestimo.LivroID <= 0)
                return BadRequest("Dados inválidos para o empréstimo.");

            try
            {
                var leitor = _context.Leitores.FirstOrDefault(l => l.Id == novoEmprestimo.LeitorID);
                if (leitor == null)
                    return NotFound("Leitor não encontrado.");

                var livro = _context.Livros.FirstOrDefault(l => l.Id == novoEmprestimo.LivroID);
                if (livro == null)
                    return NotFound("Livro não encontrado.");

                if (livro.QuantidadeDisponivel <= 0)
                    return BadRequest("Livro indisponível para empréstimo.");

                novoEmprestimo.DataEmprestimo = DateTime.UtcNow;
                novoEmprestimo.DataPrevistaDevolucao = DateTime.UtcNow.AddDays(14);
                novoEmprestimo.Status = StatusEmprestimo.Emprestado;

                livro.QuantidadeDisponivel--;
                _context.Emprestimos.Add(novoEmprestimo);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterEmprestimoPorId), new { id = novoEmprestimo.Id }, novoEmprestimo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("BuscarEmprestimoPorId/{id}")]
        public IActionResult ObterEmprestimoPorId(int id)
        {
            var emprestimo = _context.Emprestimos
                                     .Include(e => e.Leitor)
                                     .Include(e => e.Livro)
                                     .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null)
                return NotFound("Empréstimo não encontrado.");

            return Ok(emprestimo);
        }

        [HttpGet("ListarTodosEmprestimos")]
        public IActionResult ListarTodosEmprestimos()
        {
            var emprestimos = _context.Emprestimos
                                       .Include(e => e.Leitor)
                                       .Include(e => e.Livro)
                                       .ToList();

            if (!emprestimos.Any())
                return NotFound("Nenhum empréstimo encontrado.");

            return Ok(emprestimos);
        }

        [HttpPut("RegistrarDevolucao/{id}")]
        public IActionResult RegistrarDevolucao(int id)
        {
            var emprestimo = _context.Emprestimos.Include(e => e.Livro).FirstOrDefault(e => e.Id == id);
            if (emprestimo == null)
                return NotFound("Empréstimo não encontrado.");

            if (emprestimo.Status == StatusEmprestimo.Devolvido)
                return BadRequest("O livro já foi devolvido.");

            emprestimo.DataDevolucao = DateTime.UtcNow;
            emprestimo.Status = emprestimo.DataDevolucao > emprestimo.DataPrevistaDevolucao ? StatusEmprestimo.Atrasado : StatusEmprestimo.Devolvido;
            emprestimo.Livro.QuantidadeDisponivel++;

            _context.Emprestimos.Update(emprestimo);
            _context.SaveChanges();

            return Ok(emprestimo);
        }

        [HttpDelete("DeletarEmprestimo/{id}")]
        public IActionResult DeletarEmprestimo(int id)
        {
            var emprestimo = _context.Emprestimos.Find(id);
            if (emprestimo == null)
                return NotFound("Empréstimo não encontrado.");

            _context.Emprestimos.Remove(emprestimo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}