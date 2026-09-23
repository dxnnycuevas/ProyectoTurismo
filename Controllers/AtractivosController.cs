using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class AtractivosController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Atractivos";
        return View("EnConstruccion");
    }
}
