using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS___Library_Management_System.Entities
{
    public class Multa
    {
        public int Id { get; set; }

        public int EmprestimoID { get; set; }
        public Emprestimo Emprestimo { get; set; } = null!;

        public int LeitorID { get; set; }
        public Leitor Leitor { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
        public bool Pago { get; set; } = false; 
    }
}
