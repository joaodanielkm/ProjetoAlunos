using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Montador.Montadores.RelatorioDeAluno;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class AlunoController(IRepositorioAluno repositorio) : ControllerAbstrato
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
        byte[] pdf = new MontadorDeFichaDoAluno(id, _repositorio).CrieDocumento();
        string nomeArquivo = "Relatorio.pdf";
        string tipoArquivo = "application/pdf";

        return File(pdf, tipoArquivo, nomeArquivo);
    }

    public IActionResult Index(string searchString, string pesquisePor)
    {
        List<Aluno> alunos = [.. _repositorio.ObtenhaTodos()];

        if (alunos.Count == 0)
        {
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

            return View(alunos.ToList().OrderBy(a => a.Nome));
        }
    }

    public IActionResult DeletaAluno(string id)
    {
        Aluno aluno = _repositorio.Obtenha(id);

        if (aluno == null)
        {
            TempData["Mensagem"] = "Aluno não encontrado!";
            TempData["Retorno"] = false;
            return RedirectToAction("Index", "Aluno");
        }

        try
        {
            _repositorio.Remova(aluno);
            TempData["Mensagem"] = "Deletado com sucesso!";
            TempData["Retorno"] = true;
            return RedirectToAction("Index", "Aluno");
        }
        catch
        {
            TempData["Mensagem"] = "Erro ao deletar!";
            TempData["Retorno"] = false;
        }

        return View("Index", "Aluno");
    }
}