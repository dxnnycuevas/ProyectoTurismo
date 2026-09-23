using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class CategoriasController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Categorías";
        return View("EnConstruccion");
    }
}
