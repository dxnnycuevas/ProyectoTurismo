using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class AlojamientosController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Alojamientos";
        return View("EnConstruccion");
    }
}
