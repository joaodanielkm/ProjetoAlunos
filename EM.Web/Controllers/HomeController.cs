using EM.Domain.ProjetoEM.EM.Domain;
using EM.Repository;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace EM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAlunoRepository _rep;

        public HomeController(ILogger<HomeController> logger, IAlunoRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }
        //public IActionResult Pesquisar(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        var alunos = _rep.Listar();


        //        return View(alunos.Where(s => s.Nome.Contains(id)));

        //        ViewBag.Mensagem = "Sucesso";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Cadastrar");
        //        ViewBag.Mensagem = "Falha";
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
        public IActionResult Index(string searchString)
        {
            if (_rep.Listar == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var alunos = from m in _rep.Listar()
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                alunos = alunos.Where(s => s.Nome!.Contains(searchString));
            }

            return View(alunos.ToList());
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        [HttpGet]
        public IActionResult Editar(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var aluno = _rep.Selecionar(id);

            return View(aluno);


        }

        [HttpPost]
        public IActionResult Editar(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _rep.Atualizar(aluno);

                ViewBag.Mensagem = "Sucesso";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }


        [HttpPost]
        public  IActionResult Cadastrar(Aluno getAluno)
        {
            var aluno = new Aluno()
            {
                Matricula = getAluno.Matricula,
                Nome = getAluno.Nome,
                Sexo = getAluno.Sexo,
                Nascimento = getAluno.Nascimento,
                CPF = getAluno.CPF,
            };
            try
            {
               _rep.Persistir(aluno);

                ViewBag.Mensagem = "Sucesso";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = ex.Message;
            }
            return RedirectToAction("Cadastrar");

        }

        //public IActionResult Cadastrar(Aluno aluno)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    try
        //    {
        //        _rep.Persistir(aluno);

        //        ViewBag.Mensagem = "Sucesso";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Cadastrarrrrrrrrrrrrrrrrr");
        //        ViewBag.Mensagem = ex.Message;
        //    }
        //    return View();
        //}

        public IActionResult Deletar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                _rep.Excluir(id);
                ViewBag.Mensagem = "Sucesso";
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
}