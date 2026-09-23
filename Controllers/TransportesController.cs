using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class TransportesController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Transportes";
        return View("EnConstruccion");
    }
}
