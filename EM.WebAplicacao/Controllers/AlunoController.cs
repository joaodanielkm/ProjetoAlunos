using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Montador.Montadores.RelatorioDeAluno;
using EM.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EM.WebAplicacao.Controllers;

public class AlunoController(ILogger<HomeController> logger, IRepositorioAluno repositorio) : ControllerAbstrato(logger)
{
    protected IRepositorioAluno _repositorio = repositorio;

    public IActionResult EmitaTodos()
    {
        byte[] pdf = new MontadorDeListaDeAlunos().CrieDocumento();
        string nomeArquivo = "Relatorio.pdf";
        string tipoArquivo = "application/pdf";

        return File(pdf, tipoArquivo, nomeArquivo);
    }

    public IActionResult Emita(string id)
    {
        byte[] pdf = new MontadorDeFichaDoAluno(id).CrieDocumento();
        string nomeArquivo = "Relatorio.pdf";
        string tipoArquivo = "application/pdf";

        return File(pdf, tipoArquivo, nomeArquivo);
    }

    public IActionResult Index(string searchString, string pesquisePor)
    {
        List<Aluno> alunos = [.. _repositorio.ObtenhaTodos()];

        if (alunos.Count == 0)
        {
            ObtenhaViewBag("", retorno: false);
            return View();
        }

        if (pesquisePor == "matricula")
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                alunos.RemoveAll(a => !a.Matricula.ToString().Contains(searchString));
            }

            return View(alunos.ToList().OrderBy(a => a.Matricula));
        }
        else
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                alunos.RemoveAll(a => !a.Nome.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }

            ObtenhaViewBag("", retorno: true);
            return View(alunos.ToList().OrderBy(a => a.Nome));
        }
    }

    public IActionResult DeletaAluno(string id)
    {
        Aluno aluno = _repositorio.Obtenha(id);

        try
        {
            _repositorio.Remova(aluno);
            ObtenhaViewBag("Deletado com sucesso!", retorno: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Deletar");
            ObtenhaViewBag("Erro ao deletar!", retorno: false);
        }

        return RedirectToAction("Index", "Aluno");
    }
}