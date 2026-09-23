using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDonnyCuevas20210074.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Usuarios";
        return View("EnConstruccion");
    }
}
