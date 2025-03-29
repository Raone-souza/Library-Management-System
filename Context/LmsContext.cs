using Microsoft.EntityFrameworkCore;
using LMS___Library_Management_System.Entities;

namespace LMS___Library_Management_System.Context
{
    public class LmsContext : DbContext
    {
        public LmsContext(DbContextOptions<LmsContext> options) : base(options)
        {
        }

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Leitor> Leitores { get; set; }
        public DbSet<Livros> Livros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<RegistroAcoes> RegistrosAcao { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Multa> Multas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para evitar DELETE CASCADE na tabela Multa
            modelBuilder.Entity<Multa>()
                .HasOne(m => m.Leitor)
                .WithMany()
                .HasForeignKey(m => m.LeitorID)
                .OnDelete(DeleteBehavior.Restrict); // OU DeleteBehavior.NoAction
        }
    }
}
