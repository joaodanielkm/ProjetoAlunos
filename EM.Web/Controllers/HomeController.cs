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
        int matriculaUm = 1;
        int ultimaMatricula = _repositorio.GetAll().Max(a => a.Matricula);

        Aluno aluno = new()
        {
            Sexo = EnumeradorSexo.Masculino,
            Matricula = ultimaMatricula == 0 ? matriculaUm : ultimaMatricula++
        };

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

        Aluno aluno = new()
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