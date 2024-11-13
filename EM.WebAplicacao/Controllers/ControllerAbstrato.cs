using EM.Web.Controllers;
using EM.WebAplicacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EM.WebAplicacao.Controllers;

public abstract class ControllerAbstrato(ILogger<HomeController> logger) : Controller
{
    protected ILogger<HomeController> _logger = logger;

    [HttpPost]
    public virtual ActionResult Index(string searchString) => View("From [HttpPost]Index: filter on " + searchString);

    protected void ObtenhaViewBag(string menssagem, bool retorno)
    {
        ViewBag.Mensagem = menssagem;
        if (retorno)
        {
            ViewBag.Status = "true";
        }
        else
        {
            ViewBag.Status = "false";
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
    View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
