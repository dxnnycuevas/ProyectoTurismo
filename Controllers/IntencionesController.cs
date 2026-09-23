using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class IntencionesController : Controller
{
    private readonly TurismoJimaniContext _context;

    public IntencionesController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Intenciones
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Intenciones.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                (x.Nombre != null && x.Nombre.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Nombre).ToListAsync());
    }

    // GET: Intenciones/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Intenciones.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdIntencion == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Intenciones/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Intencion();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Intenciones/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Activa")] Intencion entidad)
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

    // GET: Intenciones/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Intenciones.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Intenciones/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Intenciones.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.Nombre, x => x.Descripcion, x => x.Activa);

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

    // GET: Intenciones/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Intenciones.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdIntencion == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Intenciones/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Intenciones.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Intenciones.Remove(entidad);

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

    private void Validar(Intencion entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(Intencion? entidad = null)
    {
        await Task.CompletedTask;
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
