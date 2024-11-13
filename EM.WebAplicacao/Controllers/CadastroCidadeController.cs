using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EM.WebAplicacao.Controllers;

public class CadastroCidadeController(ILogger<HomeController> logger, IRepositorioAluno repositorio) 
    : ControladorDeCadastroAbstrato<Cidade>(logger)
{

    protected IRepositorioAluno _repositorio = repositorio;

    public IActionResult Index()
        => View(ViewCadastro, new Cidade() { Codigo = 1, Descricao = "Teste"});//REFATORAR

    protected override void Grave(Cidade model)
    {
        base.Grave(model);
    }
}
