using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class EjemplosIntencionController : Controller
{
    private readonly TurismoJimaniContext _context;

    public EjemplosIntencionController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: EjemplosIntencion
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.EjemplosIntencion.Include(x => x.Intencion).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Intencion!.Nombre.Contains(buscar)
                || (x.Texto != null && x.Texto.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Intencion!.Nombre).ToListAsync());
    }

    // GET: EjemplosIntencion/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.EjemplosIntencion.Include(x => x.Intencion).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdEjemplo == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: EjemplosIntencion/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new EjemploIntencion();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: EjemplosIntencion/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdIntencion,Idioma,Texto")] EjemploIntencion entidad)
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

    // GET: EjemplosIntencion/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.EjemplosIntencion.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: EjemplosIntencion/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.EjemplosIntencion.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdIntencion, x => x.Idioma, x => x.Texto);

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

    // GET: EjemplosIntencion/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.EjemplosIntencion.Include(x => x.Intencion).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdEjemplo == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: EjemplosIntencion/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.EjemplosIntencion.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.EjemplosIntencion.Remove(entidad);

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

    private void Validar(EjemploIntencion entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(EjemploIntencion? entidad = null)
    {
        ViewBag.ListaIdIntencion = new SelectList(
            await _context.Intenciones.AsNoTracking().OrderBy(l => l.Nombre).ToListAsync(),
            "IdIntencion", "Nombre", entidad?.IdIntencion);
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
