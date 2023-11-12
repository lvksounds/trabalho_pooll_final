using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academico.Data;
using Academico.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Academico.Controllers
{
    public class AlunosController : Controller
    {
        private readonly AcademicoContext _context;

        public AlunosController(AcademicoContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public async Task<IActionResult> Index()
        {
              return _context.Alunos != null ? 
                          View(await _context.Alunos.ToListAsync()) :
                          Problem("Entity set 'AcademicoContext.Alunos'  is null.");
        }

        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            var disciplinasCadastradas = _context.AlunosDisciplinas.Join(
                    _context.Disciplinas,
                    a => a.DisciplinaId,
                    b => b.Id,
                    (a, b) => new { Nome = b.Nome, Semestre = a.Semestre, Ano = a.Ano }
                ).ToList();

            ViewBag.Disciplinas = disciplinasCadastradas;

            return View(aluno);
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoId,Nome")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }


        public IActionResult AddDiscipline()
        {
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "AlunoId", "Nome");
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nome");
                        

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDiscipline([Bind("AlunoId, DisciplinaId", "Ano", "Semestre")] AlunoDisciplina alunoDisciplina)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(alunoDisciplina);
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

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlunoId,Nome")] Aluno aluno)
        {
            if (id != aluno.AlunoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.AlunoId))
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
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alunos == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alunos == null)
            {
                return Problem("Entity set 'AcademicoContext.Alunos'  is null.");
            }
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
          return (_context.Alunos?.Any(e => e.AlunoId == id)).GetValueOrDefault();
        }
    }
}
