using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class CidadeController(IRepositorioAluno repositorio) 
    : ControladorDeCadastroAbstrato<Cidade>
{

    protected IRepositorioAluno _repositorio = repositorio;

    public IActionResult Index()
        => View(ViewCadastro, new Cidade() { Codigo = 1, Descricao = "Teste"});//REFATORAR

    protected override void Grave(Cidade model)
    {
        base.Grave(model);
    }
}
