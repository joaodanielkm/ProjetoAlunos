using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using EM.Dominio.Utilitarios;
using EM.Montador.Montadores.RelatorioDeAluno;
using EM.Web.Conversores;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class AlunoController(IRepositorioAluno repositorio) : ControladorDeCadastroAbstrato<AlunoModel>
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
        string? matricula = pesquisePor == "matricula" ? searchString : null;
        string? nome = pesquisePor == "nome" ? searchString : null;

        IEnumerable<Aluno> alunos = _repositorio.ObtenhaPor(matricula, nome);

        IEnumerable<AlunoModel> model = alunos.Select(a => a.Converta());

        if (pesquisePor == "matricula")
        {
            return View(model.OrderBy(a => a.Matricula));
        }
        else
        {
            return View(model.OrderBy(a => a.Nome));
        }
    }

    [HttpGet]
    public IActionResult Edita(string id) => View(ViewEditar, _repositorio.Obtenha(id).Converta());

    [HttpPost]
    public IActionResult Edita(AlunoModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Mensagem"] = "Verifique os dados digitados!";
            TempData["Retorno"] = false;

            return View(ViewEditar, model);
        }

        if (CpfEmUso(model))
        {
            TempData["Mensagem"] = "CPF em uso!";
            TempData["Retorno"] = false;
            return View(ViewEditar, model);
        }

        try
        {
            Aluno aluno = new(model.CPF.ToString())
            {
                Matricula = model.Matricula,
                Nome = model.Nome?.ToUpper(),
                Sexo = model.Sexo,
                Nascimento = Uteis.ConvertaData(model.Nascimento)
            };
            _repositorio.Atualize(aluno);
            TempData["Mensagem"] = "Editado com sucesso!";
            TempData["Retorno"] = true;
            return View(ViewEditar, model);
        }
        catch (System.ComponentModel.DataAnnotations.ValidationException ex)
        {
            TempData["Mensagem"] = ex.Message;
            TempData["Retorno"] = false;
            return View(ViewEditar, model);
        }
        catch
        {
            TempData["Retorno"] = false;
            TempData["Mensagem"] = "Erro ao editar!";
            return View(ViewEditar, model);
        }
    }

    public IActionResult Cadastra()
    {
        AlunoModel aluno = new()
        {
            Sexo = EnumeradorSexo.Masculino,
            Matricula = _repositorio.ObtenhaProximMatricula()
        };

        return View(ViewCadastro, aluno);
    }

    [HttpPost]
    public IActionResult Cadastra(AlunoModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Mensagem"] = "Verifique os dados digitados!";
            TempData["Retorno"] = false;

            return View(ViewCadastro,model);
        }

        if (CpfEmUso(model))
        {
            TempData["Mensagem"] = "CPF em uso!";
            TempData["Retorno"] = false;

            return View(ViewCadastro, model);
        }


        Aluno alunoJaCadastrado = _repositorio.Obtenha(model.Matricula.ToString());

        if (alunoJaCadastrado is { })
        {
            TempData["Mensagem"] = "Matricula já cadastrada!";
            TempData["Retorno"] = false;

            return View(ViewCadastro, model);
        }

        try
        {
            _repositorio.Adicione(model.Converta());
            TempData["Mensagem"] = "Cadastrado com sucesso!";
            TempData["Retorno"] = true;

        }
        catch (System.ComponentModel.DataAnnotations.ValidationException ex)
        {
            TempData["Mensagem"] = ex.Message;
            TempData["Retorno"] = false;
        }
        catch (Exception ex)
        {
            TempData["Mensagem"] = $"Erro ao cadastrar:\n {ex.Message}";
            TempData["Retorno"] = false;
        }

        return View(ViewCadastro, model);
    }

    private bool CpfEmUso(AlunoModel aluno) => _repositorio.ObtenhaPorCpf(aluno.CPF) is not null;

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
            _repositorio.Remova(aluno.Matricula);
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