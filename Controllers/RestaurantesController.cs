using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize]
public class RestaurantesController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Restaurantes";
        return View("EnConstruccion");
    }
}
