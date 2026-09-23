using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class RestaurantesController : Controller
{
    private readonly TurismoJimaniContext _context;

    public RestaurantesController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Restaurantes
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Restaurantes.Include(x => x.Lugar).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Lugar!.Nombre.Contains(buscar)
                || (x.TipoComida != null && x.TipoComida.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Lugar!.Nombre).ToListAsync());
    }

    // GET: Restaurantes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Restaurantes.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRestaurante == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Restaurantes/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Restaurante();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Restaurantes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdLugar,TipoComida,NivelPrecio,HoraApertura,HoraCierre,EnlaceMenu,ServicioDomicilio")] Restaurante entidad)
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

    // GET: Restaurantes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Restaurantes.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Restaurantes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Restaurantes.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdLugar, x => x.TipoComida, x => x.NivelPrecio, x => x.HoraApertura, x => x.HoraCierre, x => x.EnlaceMenu, x => x.ServicioDomicilio);

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

    // GET: Restaurantes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Restaurantes.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRestaurante == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Restaurantes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Restaurantes.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Restaurantes.Remove(entidad);

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

    private void Validar(Restaurante entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(Restaurante? entidad = null)
    {
        // Cada lugar solo puede tener un registro en esta tabla (relación 1:1)
        var actualIdLugar = entidad?.IdLugar ?? 0;
        ViewBag.ListaIdLugar = new SelectList(
            await _context.Lugares.AsNoTracking()
                .Where(l => l.Restaurante == null || l.IdLugar == actualIdLugar)
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
