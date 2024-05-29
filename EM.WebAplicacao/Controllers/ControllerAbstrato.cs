using EM.WebAplicacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EM.Web.Controllers;

public abstract class ControllerAbstrato : Controller
{
    [HttpPost]
    public string Index(string searchString) => "From [HttpPost]Index: filter on " + searchString;

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
