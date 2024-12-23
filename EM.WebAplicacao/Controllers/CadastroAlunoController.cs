using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EM.WebAplicacao.Controllers;

public class CadastroAlunoController(ILogger<HomeController> logger, IRepositorioAluno repositorio)
    : ControladorDeCadastroAbstrato<Aluno>(logger)
{
    protected IRepositorioAluno _repositorio = repositorio;

    public IActionResult Index() => View(ViewCadastro);

    [HttpGet]
    public IActionResult EditaAluno(string id) => View(ViewEditar, _repositorio.Obtenha(id));

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
            CPF = new CPF(editaAluno.CPF).EhValidoCPF() ? editaAluno.CPF : string.Empty,
        };

        if (Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Atualize(aluno);
                ObtenhaViewBag("Editado com sucesso!", retorno: true);

                return View(ViewEditar, editaAluno);
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
            CPF = new CPF(cadastraAluno.CPF).EhValidoCPF() ? cadastraAluno.CPF : string.Empty,
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

    //mover para locar correto
    private bool CpfEmUso(Aluno aluno) => _repositorio.Obtenha(aluno.CPF?.ToString()) is not null;
}
