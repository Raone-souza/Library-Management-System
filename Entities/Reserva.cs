using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS___Library_Management_System.Entities
{
    public class Reserva
    {
        public int Id { get; set; }

        // Relacionamento com Leitor
        public int LeitorID { get; set; }
        public Leitor Leitor { get; set; } = null!;

        // Relacionamento com Livro
        public int LivroID { get; set; }
        public Livros Livro { get; set; } = null!;

        public DateTime DataReserva { get; set; } = DateTime.UtcNow;
        public DateTime? DataExpiracao { get; set; }

        // Enum para status da reserva
        public StatusReserva Status { get; set; } = StatusReserva.Ativa;
    }

    // Enum para status da reserva
    public enum StatusReserva
    {
        Ativa,
        Cancelada,
        Expirada,
        Concluida
    }
}
