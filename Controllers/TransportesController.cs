using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class TransportesController : Controller
{
    private readonly TurismoJimaniContext _context;

    public TransportesController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Transportes
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Transportes.Include(x => x.Lugar).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Lugar!.Nombre.Contains(buscar)
                || (x.TipoTransporte != null && x.TipoTransporte.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Lugar!.Nombre).ToListAsync());
    }

    // GET: Transportes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Transportes.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdTransporte == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Transportes/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Transporte();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Transportes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdLugar,TipoTransporte,ZonaCobertura,Horario,InformacionPrecio,RequiereReserva")] Transporte entidad)
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

    // GET: Transportes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Transportes.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Transportes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Transportes.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdLugar, x => x.TipoTransporte, x => x.ZonaCobertura, x => x.Horario, x => x.InformacionPrecio, x => x.RequiereReserva);

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

    // GET: Transportes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Transportes.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdTransporte == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Transportes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Transportes.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Transportes.Remove(entidad);

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

    private void Validar(Transporte entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(Transporte? entidad = null)
    {
        // Cada lugar solo puede tener un registro en esta tabla (relación 1:1)
        var actualIdLugar = entidad?.IdLugar ?? 0;
        ViewBag.ListaIdLugar = new SelectList(
            await _context.Lugares.AsNoTracking()
                .Where(l => l.Transporte == null || l.IdLugar == actualIdLugar)
                .OrderBy(l => l.Nombre).ToListAsync(),
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
