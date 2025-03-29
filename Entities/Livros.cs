namespace LMS___Library_Management_System.Entities
{
    public class Livros
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public string Editora { get; set; } = string.Empty;
        public int QuantidadeDisponivel { get; set; }

        // Relacionamento com a categoria
        public int CategoriaID { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
