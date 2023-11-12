using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Academico.Models
{
    public class Curso
    {
        public int? Id { get; set; }
        [Required]
        public string Nome { get; set; } = String.Empty;

        public long DepartamentoId { get; set; }

        public Departamento? Departamento { get; set; }

        [IntegerValidator(MinValue = 20)]
        public int CargaHoraria { get; set; }
    }
}
