using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class RutasController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Rutas";
        return View("EnConstruccion");
    }
}
