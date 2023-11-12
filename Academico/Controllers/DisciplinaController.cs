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
    public class DisciplinaController : Controller
    {
        private readonly AcademicoContext _context;

        public DisciplinaController(AcademicoContext context)
        {
            _context = context;
        }

        // GET: Disciplina
        public async Task<IActionResult> Index()
        {
              return _context.Disciplinas != null ? 
                          View(await _context.Disciplinas.ToListAsync()) :
                          Problem("Entity set 'AcademicoContext.Disciplinas'  is null.");
        }

        // GET: Disciplina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disciplinas == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.Id == id);

            IEnumerable<Curso> CursosCadastrados = await _context.CursosDisciplinas
                .Join(
                    _context.Cursos,
                    a => a.CursoId,
                    b => b.Id,
                    (a, b) => new Curso { Nome = b.Nome, Id = a.DisciplinaId }
                    ).Where(result => result.Id == id ).ToListAsync();
            
            ViewBag.CursosCadastrados = CursosCadastrados;


            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // GET: Disciplina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CargaHoraria")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disciplina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disciplinas == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina == null)
            {
                return NotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nome,CargaHoraria")] Disciplina disciplina)
        {
            if (id != disciplina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinaExists(disciplina.Id))
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
            return View(disciplina);
        }

        // GET: Disciplina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disciplinas == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Disciplinas == null)
            {
                return Problem("Entity set 'AcademicoContext.Disciplinas'  is null.");
            }
            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina != null)
            {
                _context.Disciplinas.Remove(disciplina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaExists(int? id)
        {
          return (_context.Disciplinas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
