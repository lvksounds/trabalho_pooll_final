using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Academico.Models
{
    public class DepartamentoCursos
    {
        public int DepartamentoId { get; set; }
        public int CursoId { get; set; }
        public int Ano { get; set; }
    }
}
