using System;
using System.ComponentModel.DataAnnotations;

namespace LMS___Library_Management_System.Entities
{
    public class RegistroAcoes
    {
        public int Id { get; set; }  // Identificador único do log
        public int UsuarioID { get; set; }  // ID do administrador que realizou a ação
        public string TipoAcao { get; set; } = string.Empty; // Tipo da ação (Cadastro, Empréstimo, Exclusão, etc.)
        public DateTime DataHora { get; set; } = DateTime.UtcNow; // Data e hora da ação

        [MaxLength(500)] // Limita a 500 caracteres
        public string Descricao { get; set; } = string.Empty; // Detalhes da ação
    }
}
