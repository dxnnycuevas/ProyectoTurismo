using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class RutasController : Controller
{
    private readonly TurismoJimaniContext _context;

    public RutasController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Rutas
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Rutas.Include(x => x.LugarOrigen).Include(x => x.LugarDestino).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                (x.Nombre != null && x.Nombre.Contains(buscar))
                || x.LugarOrigen!.Nombre.Contains(buscar)
                || x.LugarDestino!.Nombre.Contains(buscar));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Nombre).ToListAsync());
    }

    // GET: Rutas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Rutas.Include(x => x.LugarOrigen).Include(x => x.LugarDestino).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRuta == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Rutas/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Ruta();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Rutas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,IdLugarOrigen,IdLugarDestino,DistanciaKilometros,DuracionMinutos,NivelDificultad,TipoTransporte,Descripcion,Instrucciones,Activa")] Ruta entidad)
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

    // GET: Rutas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Rutas.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Rutas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Rutas.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.Nombre, x => x.IdLugarOrigen, x => x.IdLugarDestino, x => x.DistanciaKilometros, x => x.DuracionMinutos, x => x.NivelDificultad, x => x.TipoTransporte, x => x.Descripcion, x => x.Instrucciones, x => x.Activa);

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

    // GET: Rutas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Rutas.Include(x => x.LugarOrigen).Include(x => x.LugarDestino).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRuta == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Rutas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Rutas.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Rutas.Remove(entidad);

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

    private void Validar(Ruta entidad)
    {
        if (entidad.IdLugarOrigen.HasValue && entidad.IdLugarOrigen == entidad.IdLugarDestino)
            ModelState.AddModelError(nameof(entidad.IdLugarDestino), "El destino debe ser diferente del origen.");
    }

    private async Task CargarListasAsync(Ruta? entidad = null)
    {
        ViewBag.ListaIdLugarOrigen = new SelectList(
            await _context.Lugares.AsNoTracking().OrderBy(l => l.Nombre).ToListAsync(),
            "IdLugar", "Nombre", entidad?.IdLugarOrigen);
        ViewBag.ListaIdLugarDestino = new SelectList(
            await _context.Lugares.AsNoTracking().OrderBy(l => l.Nombre).ToListAsync(),
            "IdLugar", "Nombre", entidad?.IdLugarDestino);
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
