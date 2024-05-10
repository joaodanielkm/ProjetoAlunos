using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Aluno");
}