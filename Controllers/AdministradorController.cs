using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/administradores")]
    public class AdministradorController : ControllerBase
    {
        private readonly LmsContext _context;

        public AdministradorController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("cadastrarAdm")]
        public IActionResult cadastrarAdm([FromBody] Administrador administrador)
        {
            if (administrador == null)
            {
                return BadRequest("Os dados do administrador são obrigatórios.");
            };

            _context.Add(administrador);
            _context.SaveChanges();
            return Ok(administrador); 
        }

        [HttpGet("buscarAdmPorNome")]
        public IActionResult ObterAdmPorNome([FromQuery] string nome)
        {
            var administrador = _context.Administradores
                .FirstOrDefault(x => x.Nome.Contains(nome));

            if (administrador == null)
                return NotFound("Administrador não encontrado.");

            return Ok(administrador);
        }


        [HttpGet("BuscarAdmPorId/{id}")]
        public IActionResult ObterAdmPorId(int id)
        {
            var administrador = _context.Administradores.Find(id);

            if (administrador == null)
                return NotFound("Administrador não encontrado.");

            return Ok(administrador);
        }

        [HttpGet("ListarTodosAdm")]
        public IActionResult ListarTodosAdm()
        {
            var administradores = _context.Administradores.ToList();

            if (administradores == null || !administradores.Any())
                return NotFound("Nenhum administrador encontrado.");

            return Ok(administradores);
        }


        [HttpPut("AtualizarAdmPorId/{id}")]
        public IActionResult AtualizarAdmPorId(int id, Administrador administrador)
        {
            var administradorBanco = _context.Administradores.Find(id);

            if (administradorBanco == null)
                return NotFound("Administrador não encontrado.");

            administradorBanco.Nome = administrador.Nome;
            administradorBanco.Email = administrador.Email;

            _context.Administradores.Update(administradorBanco);
            _context.SaveChanges();

            return Ok(administradorBanco);
        }

        [HttpPut("AtualizarAdmPorNome/{nome}")]
        public IActionResult AtualizarAdmPorNome(string nome, Administrador administrador)
        {
            var administradorBanco = _context.Administradores
                .FirstOrDefault(x => x.Nome == nome);

            if (administradorBanco == null)
                return NotFound("Administrador não encontrado.");

            administradorBanco.Nome = administrador.Nome;
            administradorBanco.Email = administrador.Email;

            _context.Administradores.Update(administradorBanco);
            _context.SaveChanges();

            return Ok(administradorBanco);
        }


        [HttpDelete("DeletarAdmPorId/{id}")]
        public IActionResult DeletarAdmPorId(int id)
        {
            var administradorBanco = _context.Administradores.Find(id);
            if (administradorBanco == null)
                return NotFound("Administrador não encontrado.");

            _context.Administradores.Remove(administradorBanco);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeletarPorNome/{nome}")]
        public IActionResult DeletarAdmPorNome(string nome)
        {
            var administradorBanco = _context.Administradores
                .FirstOrDefault(x => x.Nome == nome);

            if (administradorBanco == null)
                return NotFound("Administrador não encontrado.");

            _context.Administradores.Remove(administradorBanco);
            _context.SaveChanges();

            return NoContent();
        }




    }
}
