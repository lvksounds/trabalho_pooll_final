using System.ComponentModel.DataAnnotations;

namespace Academico.Models
{
    public class AlunoDisciplina
    {
        [Key]
        public int? AlunoId { get; set; }
        public int? DisciplinaId { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }
    }
}