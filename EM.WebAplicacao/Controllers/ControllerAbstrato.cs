using EM.WebAplicacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EM.Web.Controllers;

public abstract class ControllerAbstrato : Controller
{
    [HttpPost]
    public virtual ActionResult Index(string searchString) => View("From [HttpPost]Index: filter on " + searchString);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
    View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
