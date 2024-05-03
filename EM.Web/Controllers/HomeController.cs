using EM.Domain;
using EM.Domain.Interface;
using EM.Domain.ProjetoEM.EM.Domain;
using EM.Domain.Utilitarios;
using EM.Web.Models;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EM.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly IRepositorioAluno _repositorio;

    public HomeController(ILogger<HomeController> logger, IRepositorioAluno repositorio)
    {
        _logger = logger;
        _repositorio = repositorio;
    }

    public IActionResult Index(string? searchString, string? pesquisePor)
    {
        List<Aluno> alunos = _repositorio.GetAll().ToList();
        ObtenhaViewBag("", true);

        if (!alunos.Any())
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
                alunos.RemoveAll(a => !a.Nome.Contains(searchString.ToUpper()));
            }

            return View(alunos.ToList().OrderBy(a => a.Nome));
        }
    }

    [HttpPost]
    public string Index(string searchString) => "From [HttpPost]Index: filter on " + searchString;

    [HttpGet]
    public IActionResult Editar(string id) => View(_repositorio.Get(id));

    [HttpPost]
    public IActionResult Editar(Aluno editaAluno)
    {
        if (!ModelState.IsValid)
        {
            return View(editaAluno);
        }

        Aluno aluno = new()
        {
            Matricula = editaAluno.Matricula,
            Nome = editaAluno.Nome?.ToUpper().Trim() ?? string.Empty,
            Sexo = editaAluno.Sexo,
            Nascimento = Uteis.ConvertaData(editaAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(editaAluno.CPF) ? editaAluno.CPF : null,
        };

        if (Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Update(aluno);
                ObtenhaViewBag("Editado com sucesso!", true);

                return View(editaAluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Atualizar");
                ObtenhaViewBag("Erro ao editar!", false);
            }
        }
        return View(editaAluno);
    }

    public IActionResult Cadastrar()
    {
        int novaMatricula = 1;
        IEnumerable<Aluno> alunos = _repositorio.GetAll();

        if (alunos.Any())
        {
            novaMatricula += alunos.Max(a => a.Matricula);
        }

        Aluno aluno = new()
        {
            Sexo = EnumeradorSexo.Masculino,
            Matricula = novaMatricula
        };

        return View(aluno);
    }

    [HttpPost]
    public IActionResult Cadastrar(Aluno getAluno)
    {
        if (!ModelState.IsValid)
        {
            ObtenhaViewBag("Verifique os dados digitados!", false);
            return View(getAluno);
        }

        Aluno aluno = new()
        {
            Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : 1,
            Nome = getAluno.Nome?.ToUpper().Trim(),
            Sexo = getAluno.Sexo,
            Nascimento = Uteis.ConvertaData(getAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(getAluno.CPF) ? getAluno.CPF : "",
        };

        bool ehMatriculaJaCadastrada = _repositorio.Get(getAluno.Matricula.ToString())?.Matricula > 0;

        if (ehMatriculaJaCadastrada)
        {
            ObtenhaViewBag("Matricula já cadastrada!", false);
            return View(getAluno);
        }

        if (Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Add(aluno);
                ObtenhaViewBag("Cadastrado com sucesso!", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ObtenhaViewBag("Erro ao cadastrar!", false);
            }
        }

        return View(getAluno);
    }

    public IActionResult Deletar(string id)
    {
        Aluno? aluno = _repositorio.Get(id);

        try
        {
            _repositorio.Remove(aluno);
            ObtenhaViewBag("Deletado com sucesso!", false);
        }
        catch (FbException ex)
        {
            _logger.LogError(ex, "Deletar");
            ObtenhaViewBag("Erro ao deletar!", false);
        }

        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    private void ObtenhaViewBag(string menssagem, bool retorno)
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
}