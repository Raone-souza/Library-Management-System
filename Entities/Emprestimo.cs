using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS___Library_Management_System.Entities
{
    public class Emprestimo
    {
        public int Id { get; set; }

        // Relacionamento com Leitor
        public int LeitorID { get; set; }
        public Leitor Leitor { get; set; } = null!; // Propriedade de navegação

        // Relacionamento com Livro
        public int LivroID { get; set; }
        public Livros Livro { get; set; } = null!; // Propriedade de navegação

        public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; } // Agora é opcional

        // Enum para evitar erros de digitação
        public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Emprestado;
    }

    // Enum para status do empréstimo
    public enum StatusEmprestimo
    {
        Emprestado,
        Devolvido,
        Atrasado
    }
}
