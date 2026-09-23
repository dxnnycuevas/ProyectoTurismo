using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class ContactosController : Controller
{
    private readonly TurismoJimaniContext _context;

    public ContactosController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Contactos
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Contactos.Include(x => x.Lugar).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                x.Lugar!.Nombre.Contains(buscar)
                || (x.ValorContacto != null && x.ValorContacto.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Lugar!.Nombre).ToListAsync());
    }

    // GET: Contactos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Contactos.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdContacto == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Contactos/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Contacto();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Contactos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdLugar,TipoContacto,ValorContacto,EsPrincipal")] Contacto entidad)
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

    // GET: Contactos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Contactos.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Contactos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Contactos.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdLugar, x => x.TipoContacto, x => x.ValorContacto, x => x.EsPrincipal);

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

    // GET: Contactos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Contactos.Include(x => x.Lugar).AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdContacto == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Contactos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Contactos.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.Contactos.Remove(entidad);

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

    private void Validar(Contacto entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(Contacto? entidad = null)
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
