using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/leitores")]
    public class LeitorController : ControllerBase
    {
        private readonly LmsContext _context;

        public LeitorController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CadastrarLeitor")]
        public IActionResult CadastrarLeitor([FromBody] Leitor leitor)
        {
            if (leitor == null)
            {
                return BadRequest("Os dados do leitor são obrigatórios.");
            }

            _context.Add(leitor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterLeitorPorId), new { id = leitor.Id }, leitor);
        }

        [HttpGet("BuscarLeitorPorNome")]
        public IActionResult ObterLeitorPorNome([FromQuery] string nome)
        {
            var leitor = _context.Leitores.FirstOrDefault(x => x.Nome.Contains(nome));

            if (leitor == null)
                return NotFound("Leitor não encontrado.");

            return Ok(leitor);
        }

        [HttpGet("BuscarLeitorPorId/{id}")]
        public IActionResult ObterLeitorPorId(int id)
        {
            var leitor = _context.Leitores.Find(id);

            if (leitor == null)
                return NotFound("Leitor não encontrado.");

            return Ok(leitor);
        }

        [HttpGet("listarTodosLeitores")]
        public IActionResult ListarTodosLeitores()
        {
            var leitores = _context.Leitores.ToList();

            if (!leitores.Any())
                return NotFound("Nenhum leitor encontrado.");

            return Ok(leitores);
        }

        [HttpPut("AtualizarLeitorPorId/{id}")]
        public IActionResult AtualizarLeitorPorId(int id, [FromBody] Leitor leitor)
        {
            var LeitorBanco = _context.Leitores.Find(id);

            if (LeitorBanco == null)
                return NotFound("Leitor não encontrado.");

            LeitorBanco.Nome = leitor.Nome;
            LeitorBanco.Email = leitor.Email;
            LeitorBanco.Telefone = leitor.Telefone;

            _context.Leitores.Update(LeitorBanco);
            _context.SaveChanges();

            return Ok(LeitorBanco);
        }

        [HttpPut("AtualizarLeitorPorNome/{nome}")]
        public IActionResult AtualizarLeitorPorNome(string nome, [FromBody] Leitor leitor)
        {
            var LeitorBanco = _context.Leitores.FirstOrDefault(x => x.Nome == nome);

            if (LeitorBanco == null)
                return NotFound("Leitor não encontrado.");

            LeitorBanco.Nome = leitor.Nome;
            LeitorBanco.Email = leitor.Email;
            LeitorBanco.Telefone = leitor.Telefone;

            _context.Leitores.Update(LeitorBanco);
            _context.SaveChanges();

            return Ok(LeitorBanco);
        }

        [HttpDelete("DeletarLeitorPorId/{id}")]
        public IActionResult DeletarLeitorPorId(int id)
        {
            var LeitorBanco = _context.Leitores.Find(id);
            if (LeitorBanco == null)
                return NotFound("Leitor não encontrado.");

            _context.Leitores.Remove(LeitorBanco);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeletarLeitorPorNome/{nome}")]
        public IActionResult DeletarLeitorPorNome(string nome)
        {
            var LeitorBanco = _context.Leitores.FirstOrDefault(x => x.Nome == nome);

            if (LeitorBanco == null)
                return NotFound("Leitor não encontrado.");

            _context.Leitores.Remove(LeitorBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
