using EM.Domain;
using EM.Domain.Interface;
using EM.Domain.ProjetoEM.EM.Domain;
using EM.Domain.Utilitarios;
using EM.Web.Models;
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
    public string Index(string searchString)
    {
        return "From [HttpPost]Index: filter on " + searchString;
    }

    [HttpGet]
    public IActionResult Editar(string id)
    {
        var aluno = _repositorio.Get(id);

        return View(aluno);
    }

    [HttpPost]
    public IActionResult Editar(Aluno getAluno)
    {

        var aluno = new Aluno()
        {

            Matricula = getAluno.Matricula,
            Nome = getAluno.Nome?.ToUpper().Trim(),
            Sexo = getAluno.Sexo,
            Nascimento = Uteis.ConvertaData(getAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(getAluno.CPF) ? getAluno.CPF : null,

        };

        if (getAluno.Matricula == aluno.Matricula && !string.IsNullOrWhiteSpace(aluno.Nome) && Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Update(aluno);
                ViewBag.Mensagem = "Atualizado!";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = "erro";
            }
        }
        else
        {
            ViewBag.Mensagem = "Favor preencha o nome!";
            return View();
        }
        return View();
    }

    public IActionResult Cadastrar()
    {
        Aluno aluno = new();

        int getUltimaMatriculaMaisUm = 0;

        if (!string.IsNullOrEmpty(_repositorio.GetAll().Max(a => a.Matricula.ToString())))
        {
            getUltimaMatriculaMaisUm = _repositorio.GetAll().Max(a => a.Matricula.ToString()) == "" ? 1 : _repositorio.GetAll().Max(a => a.Matricula) + 1;
        }
        else
        {
            getUltimaMatriculaMaisUm = 1;
        }

        aluno.Sexo = EnumeradorSexo.Masculino;

        aluno.Matricula = getUltimaMatriculaMaisUm;

        return View(aluno);
    }


    [HttpPost]
    public IActionResult Cadastrar(Aluno getAluno)
    {
        int getUltimaMatriculaMaisUm = 0;

        if (!string.IsNullOrEmpty(_repositorio.GetAll().Max(a => a.Matricula.ToString())))
        {
            getUltimaMatriculaMaisUm = _repositorio.GetAll().Max(a => a.Matricula.ToString()) == "" ? 1 : _repositorio.GetAll().Max(a => a.Matricula) + 1;
        }
        else
        {
            getUltimaMatriculaMaisUm = 1;
        }

        var aluno = new Aluno()
        {
            Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : 1,
            Nome = getAluno.Nome?.ToUpper().Trim(),
            Sexo = getAluno.Sexo,
            Nascimento = Uteis.ConvertaData(getAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(getAluno.CPF) ? getAluno.CPF : "",
        };
        Aluno? verificaSeMatriculaExiste = _repositorio.Get(getAluno.Matricula.ToString());

        if (verificaSeMatriculaExiste == null && !string.IsNullOrWhiteSpace(aluno.Nome) && Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Add(aluno);
                ViewBag.Mensagem = "Cadastrado!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = "erro";
            }
        }
        else
        {
            ViewBag.Mensagem = "erro";
            return View();
        }
        return View();
    }

    public IActionResult Deletar(string id)
    {
        if (!int.TryParse(id, out int matricula))
        {
            return BadRequest("ID inválido.");
        }

        var aluno = _repositorio.GetAll().FirstOrDefault(a => a.Matricula == matricula);

        if (aluno == null)
        {
            return NotFound();
        }


        if (aluno == null || aluno.Matricula < 1)
        {
            return NotFound();
        }

        try
        {
            _repositorio.Remove(aluno);
            ViewBag.Mensagem = "Deletado";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cadastrar");
            ViewBag.Mensagem = "Falha";
        }
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}