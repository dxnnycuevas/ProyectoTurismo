using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class DocumentosConocimientoController : Controller
{
    private readonly TurismoJimaniContext _context;

    public DocumentosConocimientoController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: DocumentosConocimiento
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.DocumentosConocimiento.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                (x.Titulo != null && x.Titulo.Contains(buscar))
                || (x.Contenido != null && x.Contenido.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Titulo).ToListAsync());
    }

    // GET: DocumentosConocimiento/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.DocumentosConocimiento.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdDocumento == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: DocumentosConocimiento/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new DocumentoConocimiento();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: DocumentosConocimiento/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,TipoFuente,IdReferencia,Contenido,Activo")] DocumentoConocimiento entidad)
    {
        Validar(entidad);

        if (ModelState.IsValid)
        {
            entidad.FechaCreacion = DateTime.Now;
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

    // GET: DocumentosConocimiento/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.DocumentosConocimiento.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: DocumentosConocimiento/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.DocumentosConocimiento.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.Titulo, x => x.TipoFuente, x => x.IdReferencia, x => x.Contenido, x => x.Activo);

        Validar(entidad);

        if (actualizado && ModelState.IsValid)
        {
                entidad.FechaActualizacion = DateTime.Now;
            if (await GuardarAsync())
            {
                TempData["Exito"] = "Registro actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // GET: DocumentosConocimiento/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.DocumentosConocimiento.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdDocumento == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: DocumentosConocimiento/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.DocumentosConocimiento.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.DocumentosConocimiento.Remove(entidad);

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

    private void Validar(DocumentoConocimiento entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(DocumentoConocimiento? entidad = null)
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
