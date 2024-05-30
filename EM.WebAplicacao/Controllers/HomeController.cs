using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index(string searchString, string pesquisePor) => RedirectToAction("Index", "Aluno", new { searchString, pesquisePor });
}