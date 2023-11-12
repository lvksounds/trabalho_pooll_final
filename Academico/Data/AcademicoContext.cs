
using Academico.Models;
using Microsoft.EntityFrameworkCore;

namespace Academico.Data
{
    public class AcademicoContext : DbContext
    {
        public AcademicoContext(DbContextOptions<AcademicoContext> options) : base(options)
        {
        }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<CursoDisciplina> CursosDisciplinas { get; set; }
        public DbSet<AlunoDisciplina> AlunosDisciplinas { get; set; }

        //public DbSet<DepartamentoCursos> DepartamentoCursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new AlunoDisciplinaMap());
            //modelBuilder.ApplyConfiguration(new AlunoMap());
            //modelBuilder.ApplyConfiguration(new CursoDisciplinaMap());
            //modelBuilder.ApplyConfiguration(new CursoMap());
            //modelBuilder.ApplyConfiguration(new DepartamentoMap());
            //modelBuilder.ApplyConfiguration(new DisciplinaMap());
            //modelBuilder.ApplyConfiguration(new InstituicaoMap());

            modelBuilder.Entity<CursoDisciplina>().HasKey(cd => new { cd.DisciplinaId, cd.CursoId });
            modelBuilder.Entity<Aluno>()
                .HasMany(aluno => aluno.Disciplinas)
                .WithMany(disciplina => disciplina.Alunos)
                .UsingEntity<AlunoDisciplina>(x => x.HasKey("AlunoId", "DisciplinaId", "Semestre", "Ano"));

            //modelBuilder.Entity<DepartamentoCursos>().HasKey(cd => new { cd.CursoId, cd.DepartamentoId, cd.Ano });
                
                
        }
    }
}
