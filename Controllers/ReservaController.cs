using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/Reservas")]
    public class ReservaController : ControllerBase
    {
        private readonly LmsContext _context;

        public ReservaController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CriarReserva")]
        public IActionResult CriarReserva([FromBody] Reserva novaReserva)
        {
            if (novaReserva == null || novaReserva.LeitorID <= 0 || novaReserva.LivroID <= 0)
                return BadRequest("Dados inválidos para a reserva.");

            try
            {
                var leitor = _context.Leitores.FirstOrDefault(l => l.Id == novaReserva.LeitorID);
                if (leitor == null)
                    return NotFound("Leitor não encontrado.");

                var livro = _context.Livros.FirstOrDefault(l => l.Id == novaReserva.LivroID);
                if (livro == null)
                    return NotFound("Livro não encontrado.");

                if (livro.QuantidadeDisponivel <= 0)
                    return BadRequest("Livro indisponível para reserva.");

                novaReserva.DataExpiracao = DateTime.UtcNow.AddDays(7);

                _context.Reservas.Add(novaReserva);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterReservaPorId), new { id = novaReserva.Id }, novaReserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("BuscarReservaPorId/{id}")]
        public IActionResult ObterReservaPorId(int id)
        {
            var reserva = _context.Reservas
                                  .Include(r => r.Leitor)
                                  .Include(r => r.Livro)
                                  .FirstOrDefault(r => r.Id == id);

            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            return Ok(reserva);
        }

        [HttpGet("ListarTodasReservas")]
        public IActionResult ListarTodasReservas()
        {
            var reservas = _context.Reservas
                                   .Include(r => r.Leitor)
                                   .Include(r => r.Livro)
                                   .ToList();

            if (!reservas.Any())
                return NotFound("Nenhuma reserva encontrada.");

            return Ok(reservas);
        }

        [HttpPut("AtualizarReservaPorId/{id}")]
        public IActionResult AtualizarReservaPorId(int id, [FromBody] Reserva reservaAtualizada)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            reserva.DataReserva = reservaAtualizada.DataReserva;
            reserva.DataExpiracao = reservaAtualizada.DataExpiracao;
            reserva.Status = reservaAtualizada.Status;

            _context.Reservas.Update(reserva);
            _context.SaveChanges();

            return Ok(reserva);
        }

        [HttpPut("AtualizarReservaPorNome/{nomeLeitor}")]
        public IActionResult AtualizarReservaPorNome(string nomeLeitor, [FromBody] Reserva reservaAtualizada)
        {
            var reserva = _context.Reservas.Include(r => r.Leitor)
                                           .FirstOrDefault(r => r.Leitor.Nome.ToLower() == nomeLeitor.ToLower());

            if (reserva == null)
                return NotFound("Reserva não encontrada para o leitor informado.");

            reserva.DataReserva = reservaAtualizada.DataReserva;
            reserva.DataExpiracao = reservaAtualizada.DataExpiracao;
            reserva.Status = reservaAtualizada.Status;

            _context.Reservas.Update(reserva);
            _context.SaveChanges();

            return Ok(reserva);
        }

        [HttpPut("CancelarReservaPorId/{id}")]
        public IActionResult CancelarReservaPorId(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            reserva.Status = StatusReserva.Cancelada;
            _context.Reservas.Update(reserva);
            _context.SaveChanges();

            return Ok(reserva);
        }

        [HttpPut("CancelarReservaPorNome/{nomeLeitor}")]
        public IActionResult CancelarReservaPorNome(string nomeLeitor)
        {
            var reserva = _context.Reservas.Include(r => r.Leitor)
                                           .FirstOrDefault(r => r.Leitor.Nome.ToLower() == nomeLeitor.ToLower());

            if (reserva == null)
                return NotFound("Reserva não encontrada para o leitor informado.");

            reserva.Status = StatusReserva.Cancelada;
            _context.Reservas.Update(reserva);
            _context.SaveChanges();

            return Ok(reserva);
        }


        [HttpDelete("DeletarReservaPorId/{id}")]
        public IActionResult DeletarReservaPorId(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeletarReservaPorNome/{nomeLeitor}")]
        public IActionResult DeletarReservaPorNome(string nomeLeitor)
        {
            var reserva = _context.Reservas.Include(r => r.Leitor)
                                           .FirstOrDefault(r => r.Leitor.Nome.ToLower() == nomeLeitor.ToLower());

            if (reserva == null)
                return NotFound("Reserva não encontrada para o leitor informado.");

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
