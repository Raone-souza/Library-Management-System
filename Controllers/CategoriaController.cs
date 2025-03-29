using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;
using System.Linq;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly LmsContext _context;

        public CategoriaController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CriarCategoria")]
        public IActionResult CriarCategoria([FromBody] Categoria novaCategoria)
        {
            if (novaCategoria == null || string.IsNullOrWhiteSpace(novaCategoria.Nome))
                return BadRequest("O nome da categoria é obrigatório.");

            _context.Categorias.Add(novaCategoria);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterCategoriaPorId), new { id = novaCategoria.Id }, novaCategoria);
        }

        [HttpGet("BuscarCategoriaPorId/{id}")]
        public IActionResult ObterCategoriaPorId(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            return Ok(categoria);
        }

        [HttpGet("BuscarCategoriaPorNome")]
        public IActionResult ObterCategoriaPorNome([FromQuery] string nome)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Nome.ToLower().Contains(nome.ToLower()));

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            return Ok(categoria);
        }

        [HttpGet("ListarTodasCategorias")]
        public IActionResult ListarTodasCategorias()
        {
            var categorias = _context.Categorias.ToList();

            if (!categorias.Any())
                return NotFound("Nenhuma categoria encontrada.");

            return Ok(categorias);
        }


        [HttpPut("AtualizarCategoriaPorId/{id}")]
        public IActionResult AtualizarCategoriaPorId(int id, [FromBody] Categoria categoriaAtualizada)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            categoria.Nome = categoriaAtualizada.Nome;

            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }


        [HttpPut("AtualizarCategoriaPorNome/{nome}")]
        public IActionResult AtualizarCategoriaPorNome(string nome, [FromBody] Categoria categoriaAtualizada)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Nome.ToLower() == nome.ToLower());

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            categoria.Nome = categoriaAtualizada.Nome;

            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("DeletarCategoriaPorId/{id}")]
        public IActionResult DeletarCategoriaPorId(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeletarCategoriaPorNome")]
        public IActionResult DeletarCategoriaPorNome([FromQuery] string nome)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Nome == nome);

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
