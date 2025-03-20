using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Montador.Montadores.RelatorioDeAluno;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class AlunoController(IRepositorioAluno repositorio) : ControladorDeCadastroAbstrato<Aluno>
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

    [HttpGet]
    public IActionResult Edita(string id) => View(ViewEditar, _repositorio.Obtenha(id));

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
            Nome = editaAluno.Nome?.ToUpper(),
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
                TempData["Retorno"] = true;
                return View(ViewEditar, editaAluno);
            }
            catch
            {
                TempData["Retorno"] = false;
                TempData["Mensagem"] = "Erro ao editar!";
            }
        }
        return View(editaAluno);
    }

    public IActionResult Cadastra()
    {
        Aluno aluno = new()
        {
            Sexo = EnumeradorSexo.Masculino,
            Matricula = _repositorio.ObtenhaProximMatricula()
        };

        return View(ViewCadastro, aluno);
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
            Matricula = cadastraAluno.Matricula,
            Nome = cadastraAluno.Nome?.ToUpper().Trim(),
            Sexo = cadastraAluno.Sexo,
            Nascimento = Uteis.ConvertaData(cadastraAluno.Nascimento),
            CPF = new CPF(cadastraAluno.CPF).EhValidoCPF() ? cadastraAluno.CPF : null,
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