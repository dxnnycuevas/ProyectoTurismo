using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class AlojamientosController : Controller
{
    private readonly TurismoJimaniContext _context;

    public AlojamientosController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Alojamientos
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Alojamientos.Include(x => x.Lugar).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Lugar!.Nombre.Contains(buscar)
                || (x.TipoAlojamiento != null && x.TipoAlojamiento.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Lugar!.Nombre).ToListAsync());
    }

    // GET: Alojamientos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Alojamientos.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdAlojamiento == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Alojamientos/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Alojamiento();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Alojamientos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdLugar,TipoAlojamiento,PrecioMinimo,PrecioMaximo,CantidadHabitaciones,HoraEntrada,HoraSalida,EnlaceReserva")] Alojamiento entidad)
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

    // GET: Alojamientos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Alojamientos.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Alojamientos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Alojamientos.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdLugar, x => x.TipoAlojamiento, x => x.PrecioMinimo, x => x.PrecioMaximo, x => x.CantidadHabitaciones, x => x.HoraEntrada, x => x.HoraSalida, x => x.EnlaceReserva);

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

    // GET: Alojamientos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Alojamientos.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdAlojamiento == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Alojamientos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Alojamientos.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Alojamientos.Remove(entidad);

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

    private void Validar(Alojamiento entidad)
    {
        if (entidad.PrecioMinimo.HasValue && entidad.PrecioMaximo.HasValue && entidad.PrecioMinimo > entidad.PrecioMaximo)
            ModelState.AddModelError(nameof(entidad.PrecioMaximo), "El precio máximo debe ser mayor o igual que el mínimo.");
    }

    private async Task CargarListasAsync(Alojamiento? entidad = null)
    {
        // Cada lugar solo puede tener un registro en esta tabla (relación 1:1)
        var actualIdLugar = entidad?.IdLugar ?? 0;
        ViewBag.ListaIdLugar = new SelectList(
            await _context.Lugares.AsNoTracking()
                .Where(l => l.Alojamiento == null || l.IdLugar == actualIdLugar)
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
