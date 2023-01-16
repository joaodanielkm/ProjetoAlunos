using EM.Domain.ProjetoEM.EM.Domain;
using EM.Repository;
using EM.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.WebPages;

namespace EM.Web.Controllers
{
    public class HomeController : Controller
    {
        Domain.Utilitarios.Uteis uteis = new Domain.Utilitarios.Uteis();
        private readonly ILogger<HomeController> _logger;
        public readonly IAlunoRepository _rep;
        Aluno.ExisteMatricula aluno1 = new Aluno.ExisteMatricula();

        public HomeController(ILogger<HomeController> logger, IAlunoRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public IActionResult Index(string searchString, string pesquisePor)
        {
                if (pesquisePor == "matricula")
                {
                    var alunosPorMatricula = from a in _rep.Listar()
                                             select a;
                if (!String.IsNullOrEmpty(searchString))
                {
                    alunosPorMatricula = alunosPorMatricula.Where(a => a.Matricula.ToString().Contains(searchString));
                } 

                    return View(alunosPorMatricula.ToList());
                }
                else
                {

                    if (_rep.Listar == null)
                    {
                        return Problem("Aluno não encontrado!");
                    }

                    var alunosPorNome = from a in _rep.Listar()
                                 select a;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                    alunosPorNome = alunosPorNome.Where(a => a.Nome!.Contains(searchString));
                    }

                    return View(alunosPorNome.ToList());
                }
            }
        
        [HttpPost]
        public string Index(string searchString, string pesquisePor, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + pesquisePor + searchString ;
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

        //[HttpPost]
        //public IActionResult Editar(Aluno getAluno)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    var aluno = new Aluno()
        //    {

        //        Matricula = getAluno.Matricula,
        //        Nome = getAluno.Nome,
        //        Sexo = getAluno.Sexo,
        //        Nascimento = uteis.ConvertaData(getAluno.Nascimento),
        //        CPF = getAluno.CPF,

        //    };
        //    try
        //    {
        //        _rep.Atualizar(aluno);

        //        ViewBag.Mensagem = "Sucesso";
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Cadastrar");
        //        ViewBag.Mensagem = ex.Message;
        //    }
        //    return View();
        //}


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
                return RedirectToAction("Index", "Home");
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

        //[HttpGet]
        //public IActionResult Cadastrar( )
        //{
        //    Aluno aluno = new Aluno();

        //    var getUltimaMatricula = _rep.Listar().Max(m => m.Matricula + 1);

        //    aluno.Matricula = getUltimaMatricula;

        //    return View();
        //}


        [HttpPost]
        public  IActionResult Cadastrar(Aluno getAluno)
        {
            var getMatriculas = from a in _rep.Listar()
                                select a;

            int mat = getAluno.Matricula;


            //getMatriculas = getMatriculas.Where(a => a.Matricula == mat);


            var getUltimaMatricula = _rep.Listar().Max(a => a.Matricula + 1) ;

             aluno1.UltimaMatricula = Convert.ToInt32(mat);//erro de cast aqui-verificar para validar matricula já existente.

            var aluno = new Aluno()
            {
                Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : getUltimaMatricula,
                Nome = getAluno.Nome,
                Sexo = getAluno.Sexo,
                Nascimento = uteis.ConvertaData(getAluno.Nascimento),
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