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
        if (pesquisePor == "matricula")
        {
            var alunosPorMatricula = from a in _repositorio.GetAll()
                                     select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                alunosPorMatricula = alunosPorMatricula.Where(a => a.Matricula.ToString().Contains(searchString));
            }

            return View(alunosPorMatricula.ToList().OrderBy(a => a.Matricula));
        }
        else
        {

            if (_repositorio.GetAll == null)
            {
                return Problem();
            }

            var alunosPorNome = from a in _repositorio.GetAll()
                                select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                alunosPorNome = alunosPorNome.Where(a => a.Nome!.Contains(searchString.ToUpper()));
            }
            return View(alunosPorNome.ToList().OrderBy(a => a.Nome));
        }
    }

    [HttpPost]
    public string Index(string searchString) => "From [HttpPost]Index: filter on " + searchString;

    [HttpGet]
    public IActionResult Editar(string id) => View(_repositorio.Get(id));

    [HttpPost]
    public IActionResult Editar(Aluno editaAluno)
    {
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
                RetorneTrue();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Atualizar");
                Retornefalse();
            }
        }
        return View();
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
        Aluno aluno = new()
        {
            Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : 1,
            Nome = getAluno.Nome?.ToUpper().Trim(),
            Sexo = getAluno.Sexo,
            Nascimento = Uteis.ConvertaData(getAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(getAluno.CPF) ? getAluno.CPF : "",
        };

        bool existeMatricula = _repositorio.Get(getAluno.Matricula.ToString())?.Matricula == 0;

        if (!existeMatricula && Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Add(aluno);
                RetorneTrue();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                Retornefalse();
            }
        }
        return View();
    }

    public IActionResult Deletar(string id)
    {
        Aluno? aluno = _repositorio.Get(id);

        if (aluno is null)
        {
            return NotFound();
        }

        try
        {
            _repositorio.Remove(aluno);
            RetorneTrue();
        }
        catch (FbException ex)
        {
            _logger.LogError(ex, "Deletar");
            Retornefalse();
        }
        return View();//RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    private string RetorneTrue() => ViewBag.Mensagem = "true";

    private string Retornefalse() => ViewBag.Mensagem = "false";
}