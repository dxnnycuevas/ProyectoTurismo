using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class AsistenteController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Asistente";
        return View("EnConstruccion");
    }
}
