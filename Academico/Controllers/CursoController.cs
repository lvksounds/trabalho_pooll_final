using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academico.Data;
using Academico.Models;

namespace Academico.Controllers
{
    public class CursoController : Controller
    {
        private readonly AcademicoContext _context;

        public CursoController(AcademicoContext context)
        {
            _context = context;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
              return _context.Cursos != null ? 
                          View(await _context.Cursos.ToListAsync()) :
                          Problem("Entity set 'AcademicoContext.Cursos'  is null.");
        }

        // GET: Curso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            IEnumerable<CursoDisciplina> disciplinasCurso = await _context.CursosDisciplinas.Where(d => d.CursoId == id).ToListAsync();

            IEnumerable<Disciplina> disciplinasCadastradas = await _context.CursosDisciplinas.Join(
                    _context.Disciplinas,
                    a => a.DisciplinaId,
                    b => b.Id,
                    (a, b) => new Disciplina { Nome = b.Nome, Id = a.CursoId}
                ).Where(result => result.Id == id).ToListAsync();

            ViewBag.Disciplinas = disciplinasCadastradas;

            return View(curso);
        }

        // GET: Curso/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoID"] = new SelectList(_context.Departamentos, "Id", "Nome");
            
            return View();
        }

        // POST: Curso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CargaHoraria, DepartamentoId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        public IActionResult AddDiscipline()
        {
            ViewBag.CursoId = new SelectList(_context.Cursos, "Id", "Nome");
            ViewBag.DisciplinaId = new SelectList(_context.Disciplinas, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDiscipline([Bind("CursoId, DisciplinaId")] CursoDisciplina cursoDisciplina)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cursoDisciplina);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    ErrorPartialModel errorView = new ErrorPartialModel(ex.Source, ex.Message);
                    
                    return PartialView("~/Views/Shared/_ErrorView.cshtml", errorView);
                }
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Curso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: Curso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nome,CargaHoraria")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: Curso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Cursos == null)
            {
                return Problem("Entity set 'AcademicoContext.Cursos'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int? id)
        {
          return (_context.Cursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
