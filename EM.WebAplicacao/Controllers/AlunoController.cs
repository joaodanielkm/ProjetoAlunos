using EM.Dominio;
using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Montador.Montadores.Aluno;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class AlunoController(ILogger<HomeController> logger, IRepositorioAluno repositorio) : ControllerAbstrato
{
    private readonly ILogger<HomeController> _logger = logger;
    public readonly IRepositorioAluno _repositorio = repositorio;

    public IActionResult EmitirTodos()
    {
        byte[] pdfContent = new MontadorDeRelatorioDoAluno().CrieDocumento();
        string fileName = "Relatorio.pdf";
        string contentType = "application/pdf";

        return File(pdfContent, contentType, fileName);
    }

    public IActionResult Index(string searchString, string pesquisePor)
    {
        List<Aluno> alunos = _repositorio.GetAll().ToList();

        if (alunos.Count == 0)
        {
            ObtenhaViewBag("", false);
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

            ObtenhaViewBag("", true);
            return View(alunos.ToList().OrderBy(a => a.Nome));
        }
    }

    [HttpGet]
    public IActionResult EditaAluno(string id) => View(_repositorio.Get(id));

    [HttpPost]
    public IActionResult EditaAluno(Aluno editaAluno)
    {
        if (!ModelState.IsValid)
        {
            return View(editaAluno);
        }

        if (CpfEmUso(editaAluno))
        {
            ObtenhaViewBag("CPF em uso!", false);
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

    public IActionResult CadastraAluno()
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
    public IActionResult CadastraAluno(Aluno cadastraAluno)
    {
        if (!ModelState.IsValid)
        {
            ObtenhaViewBag("Verifique os dados digitados!", false);
            return View(cadastraAluno);
        }

        if (CpfEmUso(cadastraAluno))
        {
            ObtenhaViewBag("CPF em uso!", false);
            return View(cadastraAluno);
        }

        if (!Uteis.EhValidoNome(cadastraAluno.Nome))
        {
            ObtenhaViewBag($"Verifique o nome cadastrado.", false);
            return View(cadastraAluno);
        }

        Aluno alunoJaCadastrado = _repositorio.Get(cadastraAluno.Matricula.ToString());

        if (alunoJaCadastrado is not null)
        {
            ObtenhaViewBag("Matricula já cadastrada!", false);
            return View(cadastraAluno);
        }


        Aluno aluno = new()
        {
            Matricula = (cadastraAluno.Matricula > 0) ? cadastraAluno.Matricula : 1,
            Nome = cadastraAluno.Nome?.ToUpper().Trim(),
            Sexo = cadastraAluno.Sexo,
            Nascimento = Uteis.ConvertaData(cadastraAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(cadastraAluno.CPF) ? cadastraAluno.CPF : "",
        };

        try
        {
            _repositorio.Add(aluno);
            ObtenhaViewBag("Cadastrado com sucesso!", true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cadastrar");
            ObtenhaViewBag($"Erro ao cadastrar:\n {ex.Message}", false);
        }

        return View(cadastraAluno);
    }

    public IActionResult DeletaAluno(string id)
    {
        Aluno aluno = _repositorio.Get(id);

        try
        {
            _repositorio.Remove(aluno);
            ObtenhaViewBag("Deletado com sucesso!", false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Deletar");
            ObtenhaViewBag("Erro ao deletar!", false);
        }

        return RedirectToAction("Index", "Aluno");
    }

    private bool CpfEmUso(Aluno aluno) => _repositorio.Get(aluno.CPF?.ToString()) is not null;

}