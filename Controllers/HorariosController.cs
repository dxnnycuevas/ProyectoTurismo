using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class HorariosController : Controller
{
    private readonly TurismoJimaniContext _context;

    public HorariosController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Horarios
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Horarios.Include(x => x.Lugar).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Lugar!.Nombre.Contains(buscar));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Lugar!.Nombre).ThenBy(x => x.DiaSemana).ToListAsync());
    }

    // GET: Horarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Horarios.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdHorario == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Horarios/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Horario();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Horarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdLugar,DiaSemana,HoraApertura,HoraCierre,Cerrado")] Horario entidad)
    {
        Validar(entidad);

        if (ModelState.IsValid)
        {
            _context.Add(entidad);

            if (await GuardarAsync())
            {
                TempData["Exito"] = "Registro creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // GET: Horarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Horarios.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Horarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Horarios.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdLugar, x => x.DiaSemana, x => x.HoraApertura, x => x.HoraCierre, x => x.Cerrado);

        Validar(entidad);

        if (actualizado && ModelState.IsValid)
        {
            if (await GuardarAsync())
            {
                TempData["Exito"] = "Registro actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // GET: Horarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Horarios.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdHorario == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Horarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Horarios.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Horarios.Remove(entidad);

        try
        {
            await _context.SaveChangesAsync();
            TempData["Exito"] = "Registro eliminado correctamente.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se puede eliminar el registro porque está relacionado con otros datos.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void Validar(Horario entidad)
    {
        if (!entidad.Cerrado && entidad.HoraApertura.HasValue && entidad.HoraCierre.HasValue && entidad.HoraCierre <= entidad.HoraApertura)
            ModelState.AddModelError(nameof(entidad.HoraCierre), "La hora de cierre debe ser posterior a la de apertura.");
    }

    private async Task CargarListasAsync(Horario? entidad = null)
    {
        ViewBag.ListaIdLugar = new SelectList(
            await _context.Lugares.AsNoTracking().OrderBy(l => l.Nombre).ToListAsync(),
            "IdLugar", "Nombre", entidad?.IdLugar);
    }

    private async Task<bool> GuardarAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty,
                "No se pudo guardar. Verifique que no exista otro registro con los mismos datos.");
            return false;
        }
    }
}
