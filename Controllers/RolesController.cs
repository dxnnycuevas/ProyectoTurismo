using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize(Roles = "Administrador")]
public class RolesController : Controller
{
    private readonly TurismoJimaniContext _context;

    public RolesController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: Roles
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                (x.Nombre != null && x.Nombre.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(x => x.Nombre).ToListAsync());
    }

    // GET: Roles/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRol == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: Roles/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new Rol();
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre")] Rol entidad)
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

    // GET: Roles/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Roles.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        var entidad = await _context.Roles.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.Nombre);

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

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdRol == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entidad = await _context.Roles.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        if (await _context.Usuarios.AnyAsync(u => u.IdRol == id))
        {
            TempData["Error"] = "No se puede eliminar el rol porque tiene usuarios asignados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Roles.Remove(entidad);

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

    private void Validar(Rol entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(Rol? entidad = null)
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
