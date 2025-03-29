namespace LMS___Library_Management_System.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        // Relacionamento com Livros (Uma Categoria pode ter vários Livros)
        public List<Livros> Livros { get; set; } = new List<Livros>();
    }
}
