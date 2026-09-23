using AppDonnyCuevas20210074.Data;
using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class ConsultasAsistenteController : Controller
{
    private readonly TurismoJimaniContext _context;

    public ConsultasAsistenteController(TurismoJimaniContext context)
    {
        _context = context;
    }

    // GET: ConsultasAsistente
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _context.ConsultasAsistente.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(x =>
                (x.MensajeUsuario != null && x.MensajeUsuario.Contains(buscar))
                || (x.IntencionDetectada != null && x.IntencionDetectada.Contains(buscar)));
        }

        ViewBag.Buscar = buscar;
        return View(await consulta.OrderByDescending(x => x.FechaConsulta).ToListAsync());
    }

    // GET: ConsultasAsistente/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.ConsultasAsistente.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdConsulta == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // GET: ConsultasAsistente/Create
    public async Task<IActionResult> Create()
    {
        var entidad = new ConsultaAsistente { IdSesion = Guid.NewGuid() };
        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: ConsultasAsistente/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdSesion,MensajeUsuario,IntencionDetectada,TipoRespuesta,RespuestaAsistente,RespuestaCorrecta,TiempoRespuestaMilisegundos")] ConsultaAsistente entidad)
    {
        Validar(entidad);

        if (ModelState.IsValid)
        {
            entidad.FechaConsulta = DateTime.Now;
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

    // GET: ConsultasAsistente/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.ConsultasAsistente.FindAsync(id);
        if (entidad == null) return NotFound();

        await CargarListasAsync(entidad);
        return View(entidad);
    }

    // POST: ConsultasAsistente/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id)
    {
        var entidad = await _context.ConsultasAsistente.FindAsync(id);
        if (entidad == null) return NotFound();

        // Solo se actualizan los campos del formulario
        var actualizado = await TryUpdateModelAsync(entidad, string.Empty,
            x => x.IdSesion, x => x.MensajeUsuario, x => x.IntencionDetectada, x => x.TipoRespuesta, x => x.RespuestaAsistente, x => x.RespuestaCorrecta, x => x.TiempoRespuestaMilisegundos);

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

    // GET: ConsultasAsistente/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null) return NotFound();

        var entidad = await _context.ConsultasAsistente.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdConsulta == id);

        if (entidad == null) return NotFound();

        return View(entidad);
    }

    // POST: ConsultasAsistente/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var entidad = await _context.ConsultasAsistente.FindAsync(id);
        if (entidad == null) return RedirectToAction(nameof(Index));

        _context.ConsultasAsistente.Remove(entidad);

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

    private void Validar(ConsultaAsistente entidad)
    {
        // Sin validaciones adicionales
    }

    private async Task CargarListasAsync(ConsultaAsistente? entidad = null)
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
