using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Montador.Montadores.RelatorioDeAluno;
using EM.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EM.WebAplicacao.Controllers;

public class AlunoController(ILogger<HomeController> logger, IRepositorioAluno repositorio) : ControllerAbstrato
{
    private readonly ILogger<HomeController> _logger = logger;
    public readonly IRepositorioAluno _repositorio = repositorio;

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

    [HttpGet]
    public IActionResult EditaAluno(string id) => View(_repositorio.Obtenha(id));

    [HttpPost]
    public IActionResult EditaAluno(Aluno editaAluno)
    {
        if (!ModelState.IsValid)
        {
            return View(editaAluno);
        }

        if (CpfEmUso(editaAluno))
        {
            ObtenhaViewBag("CPF em uso!", retorno: false);
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
                _repositorio.Atualize(aluno);
                ObtenhaViewBag("Editado com sucesso!", retorno: true);

                return View(editaAluno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Atualizar");
                ObtenhaViewBag("Erro ao editar!", retorno: false);
            }
        }
        return View(editaAluno);
    }

    public IActionResult CadastraAluno()
    {
        int novaMatricula = 1;
        IEnumerable<Aluno> alunos = _repositorio.ObtenhaTodos();

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
            ObtenhaViewBag("Verifique os dados digitados!", retorno: false);
            return View(cadastraAluno);
        }

        if (CpfEmUso(cadastraAluno))
        {
            ObtenhaViewBag("CPF em uso!", retorno: false);
            return View(cadastraAluno);
        }

        if (!Uteis.EhValidoNome(cadastraAluno.Nome))
        {
            ObtenhaViewBag($"Verifique o nome cadastrado.", retorno: false);
            return View(cadastraAluno);
        }

        Aluno alunoJaCadastrado = _repositorio.Obtenha(nameof(cadastraAluno.Matricula));

        if (alunoJaCadastrado is not null)
        {
            ObtenhaViewBag("Matricula já cadastrada!", retorno: false);
            return View(cadastraAluno);
        }

        Aluno aluno = new()
        {
            Matricula = cadastraAluno.Matricula > 0 ? cadastraAluno.Matricula : 1,
            Nome = cadastraAluno.Nome?.ToUpper().Trim(),
            Sexo = cadastraAluno.Sexo,
            Nascimento = Uteis.ConvertaData(cadastraAluno.Nascimento),
            CPF = Uteis.EhValidoCPF(cadastraAluno.CPF) ? cadastraAluno.CPF : "",
        };

        try
        {
            _repositorio.Adicione(aluno);
            ObtenhaViewBag("Cadastrado com sucesso!", retorno: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cadastrar");
            ObtenhaViewBag($"Erro ao cadastrar:\n {ex.Message}", retorno: false);
        }

        return View(cadastraAluno);
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

    private bool CpfEmUso(Aluno aluno) => _repositorio.Obtenha(aluno.CPF?.ToString()) is not null;

}