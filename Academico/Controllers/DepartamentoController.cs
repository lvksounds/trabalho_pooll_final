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
    public class DepartamentoController : Controller
    {
        private readonly AcademicoContext _context;

        public DepartamentoController(AcademicoContext context)
        {
            _context = context;
        }

        // GET: Departamento
        public async Task<IActionResult> Index()
        {
            var academicoContext = _context.Departamentos.Include(d => d.Instituicao);
            return View(await academicoContext.ToListAsync());
        }

        // GET: Departamento/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .Include(d => d.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);


            departamento = await _context.Departamentos.Include(d => d.Cursos).FirstOrDefaultAsync(c => c.Id == id);

            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // GET: Departamento/Create
        public IActionResult Create()
        {
            ViewData["InstituicaoID"] = new SelectList(_context.Instituicoes, "InstituicaoId", "Nome");
            return View();
        }

        // POST: Departamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,InstituicaoID")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstituicaoID"] = new SelectList(_context.Instituicoes, "InstituicaoId", "InstituicaoId", departamento.InstituicaoID);
            return View(departamento);
        }

        //public IActionResult AddCourse()
        //{
        //    ViewData["DepartamentosId"] = new SelectList(_context.Departamentos, "Id", "Nome");
        //    ViewData["CursosId"] = new SelectList(_context.Cursos, "Id", "Nome");


        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddCourse([Bind("DepartamentoId, CursoId", "Ano")] DepartamentoCursos departamentoCurso)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(departamentoCurso);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Departamento/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            ViewData["InstituicaoID"] = new SelectList(_context.Instituicoes, "InstituicaoID", "InstituicaoID", departamento.InstituicaoID);
            return View(departamento);
        }

        // POST: Departamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("Id,Nome,InstituicaoID")] Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Id))
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
            ViewData["InstituicaoID"] = new SelectList(_context.Instituicoes, "InstituicaoID", "InstituicaoID", departamento.InstituicaoID);
            return View(departamento);
        }

        // GET: Departamento/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .Include(d => d.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // POST: Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (_context.Departamentos == null)
            {
                return Problem("Entity set 'AcademicoContext.Departamentos'  is null.");
            }
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentoExists(long? id)
        {
          return (_context.Departamentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
