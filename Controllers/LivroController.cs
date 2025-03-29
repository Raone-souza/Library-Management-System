using Microsoft.AspNetCore.Mvc;
using LMS___Library_Management_System.Context;
using LMS___Library_Management_System.Entities;

namespace LMS___Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivroController : ControllerBase
    {
        private readonly LmsContext _context;

        public LivroController(LmsContext context)
        {
            _context = context;
        }

        [HttpPost("CadastrarLivro")]
        public IActionResult CadastrarLivro([FromBody] Livros livro)
        {
            if (livro == null)
                return BadRequest("Os dados do livro são obrigatórios.");

            _context.Livros.Add(livro);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterLivroPorId), new { id = livro.Id }, livro);
        }

        [HttpGet("BuscarLivroPorTitulo")]
        public IActionResult ObterLivroPorTitulo([FromQuery] string titulo)
        {
            var livros = _context.Livros.Where(x => x.Titulo.Contains(titulo)).ToList();

            if (!livros.Any())
                return NotFound("Nenhum livro encontrado com esse título.");

            return Ok(livros);
        }

        [HttpGet("BuscarLivroPorId/{id}")]
        public IActionResult ObterLivroPorId(int id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null)
                return NotFound("Livro não encontrado.");

            return Ok(livro);
        }

        [HttpGet("listarTodosLivros")]
        public IActionResult ListarTodosLivros()
        {
            var livros = _context.Livros.ToList();

            if (!livros.Any())
                return NotFound("Nenhum livro encontrado.");

            return Ok(livros);
        }

        [HttpPut("AtualizarLivroPorId/{id}")]
        public IActionResult AtualizarLivroPorId(int id, [FromBody] Livros livroAtualizado)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null)
                return NotFound("Livro não encontrado.");

            livro.Titulo = livroAtualizado.Titulo;
            livro.Autor = livroAtualizado.Autor;
            livro.ISBN = livroAtualizado.ISBN;
            livro.AnoPublicacao = livroAtualizado.AnoPublicacao;
            livro.Editora = livroAtualizado.Editora;
            livro.QuantidadeDisponivel = livroAtualizado.QuantidadeDisponivel;
            livro.CategoriaID = livroAtualizado.CategoriaID;

            _context.Livros.Update(livro);
            _context.SaveChanges();

            return Ok(livro);
        }

        [HttpPut("AtualizarLivroPorTitulo/{titulo}")]
        public IActionResult AtualizarLivroPorTitulo(string titulo, [FromBody] Livros livroAtualizado)
        {
            var livro = _context.Livros.FirstOrDefault(x => x.Titulo.ToLower() == titulo.ToLower());

            if (livro == null)
                return NotFound("Livro não encontrado.");

            livro.Titulo = livroAtualizado.Titulo;
            livro.Autor = livroAtualizado.Autor;
            livro.ISBN = livroAtualizado.ISBN;
            livro.AnoPublicacao = livroAtualizado.AnoPublicacao;
            livro.Editora = livroAtualizado.Editora;
            livro.QuantidadeDisponivel = livroAtualizado.QuantidadeDisponivel;
            livro.CategoriaID = livroAtualizado.CategoriaID;

            _context.Livros.Update(livro);
            _context.SaveChanges();

            return Ok(livro);
        }

        [HttpDelete("DeletarLivroPorId/{id}")]
        public IActionResult DeletarLivroPorId(int id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null)
                return NotFound("Livro não encontrado.");

            _context.Livros.Remove(livro);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeletarLivroPorTitulo/{titulo}")]
        public IActionResult DeletarPorTitulo(string titulo)
        {
            var livro = _context.Livros.FirstOrDefault(x => x.Titulo.ToLower() == titulo.ToLower());

            if (livro == null)
                return NotFound("Livro não encontrado.");

            _context.Livros.Remove(livro);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
