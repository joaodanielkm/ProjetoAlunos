using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class CadastroAlunoController(IRepositorioAluno repositorio)
    : ControladorDeCadastroAbstrato<Aluno>
{
    protected IRepositorioAluno _repositorio = repositorio;

    public IActionResult Index() => View(ViewCadastro);

    [HttpGet]
    public IActionResult EditaAluno(string id) => View(ViewEditar, _repositorio.Obtenha(id));

    [HttpPost]
    public IActionResult Edita(Aluno editaAluno)
    {
        if (!ModelState.IsValid)
        {
            return View(editaAluno);
        }

        if (CpfEmUso(editaAluno))
        {
            TempData["Mensagem"] = "CPF em uso!";
            TempData["Retorno"] = false;
            return View(editaAluno);
        }

        Aluno aluno = new()
        {
            Matricula = editaAluno.Matricula,
            Nome = editaAluno.Nome?.ToUpper().Trim() ?? string.Empty,
            Sexo = editaAluno.Sexo,
            Nascimento = Uteis.ConvertaData(editaAluno.Nascimento),
            CPF = new CPF(editaAluno.CPF).EhValidoCPF() ? editaAluno.CPF : null,
        };

        if (Uteis.EhValidoNome(aluno.Nome))
        {
            try
            {
                _repositorio.Atualize(aluno);
                TempData["Mensagem"] = "Editado com sucesso!";
                return View(ViewEditar, editaAluno);
            }
            catch 
            {
                TempData["Mensagem"] = "Erro ao editar!";
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
            TempData["Mensagem"] = "Verifique os dados digitados!";
            TempData["Retorno"] = false;

            return View(cadastraAluno);
        }

        if (CpfEmUso(cadastraAluno))
        {
            TempData["Mensagem"] = "CPF em uso!";
            TempData["Retorno"] = false;

            return View(cadastraAluno);
        }

        if (!Uteis.EhValidoNome(cadastraAluno.Nome))
        {
            TempData["Mensagem"] = "Verifique o nome cadastrado.";
            TempData["Retorno"] = false;

            return View(cadastraAluno);
        }

        Aluno alunoJaCadastrado = _repositorio.Obtenha(nameof(cadastraAluno.Matricula));

        if (alunoJaCadastrado is not null)
        {
            TempData["Mensagem"] = "Matricula já cadastrada!";
            TempData["Retorno"] = false;

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
            TempData["Mensagem"] = "Cadastrado com sucesso!";
            TempData["Retorno"] = false;

        }
        catch (Exception ex)
        {
            TempData["Mensagem"] = $"Erro ao cadastrar:\n {ex.Message}";
            TempData["Retorno"] = false;
        }

        return View(cadastraAluno);
    }

    //mover para locar correto
    private bool CpfEmUso(Aluno aluno) => _repositorio.Obtenha(aluno.CPF?.ToString()) is not null;
}
