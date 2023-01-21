using EM.Domain;
using EM.Domain.ProjetoEM.EM.Domain;
using EM.Repository;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

                return View(alunosPorMatricula.ToList().OrderBy(a => a.Matricula));
            }
            else
            {

                if (_rep.Listar == null)
                {
                    return Problem();
                }

                var alunosPorNome = from a in _rep.Listar()
                                    select a;

                if (!String.IsNullOrEmpty(searchString))
                {
                    alunosPorNome = alunosPorNome.Where(a => a.Nome!.Contains(searchString));
                }


                return View(alunosPorNome.ToList().OrderBy(a => a.Nome));
            }
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        [HttpGet]
        public IActionResult Editar(string id)
        {
            var aluno = _rep.Selecionar(id);


            return View(aluno);

        }

        [HttpPost]
        public IActionResult Editar111111111111111111111(Aluno getAluno)
        {

                var aluno = new Aluno()
                {

                    Matricula = getAluno.Matricula,
                    Nome = (getAluno.Nome).ToUpper(),
                    Sexo = getAluno.Sexo,
                    Nascimento = getAluno.Nascimento,
                    CPF = (uteis.EhValidoCPF(getAluno.CPF)) ? getAluno.CPF : null,

                };
            try
            {
                _rep.Atualizar(aluno);
                
                ViewBag.Mensagem = "Sucesso";
                Thread.Sleep(2000);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = ex.Message;
            }
            return View();
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

                //return RedirectToAction("Index", "Home");
                ViewBag.Mensagem = "Atualizado!";
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
            Aluno aluno = new Aluno();

            aluno.Sexo = Sexo.Masculino;

            return View();
        }

        [HttpPost]
        public void Salvar(Aluno aluno)
        {
            Console.WriteLine(aluno.Sexo);
        }

        [HttpPost]
        public IActionResult Cadastrar(Aluno getAluno)
        {
            var getMatriculas = from a in _rep.Listar()
                                select a;

            int mat = getAluno.Matricula;
            var getUltimaMatriculaMaisUm = 0;
            if (getAluno.Matricula == null || getAluno.Matricula < 1)
            {
                getAluno.Matricula = 1;
            }
            else
            {
                getUltimaMatriculaMaisUm = _rep.Listar().Max(a => a.Matricula)+1;
                //getUltimaMatriculaMaisUm = (getUltimaMatriculaMaisUm < 1) ? 1 : getUltimaMatriculaMaisUm;
            }


            aluno1.UltimaMatricula = Convert.ToInt32(mat);//erro de cast aqui-verificar para validar matricula já existente.

            var aluno = new Aluno()
            {
                Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : getUltimaMatriculaMaisUm,
                Nome = getAluno.Nome.ToUpper(),
                Sexo = getAluno.Sexo,
                Nascimento = getAluno.Nascimento,
                CPF = (uteis.EhValidoCPF(getAluno.CPF)) ? getAluno.CPF : "",
            };
            try
            {
                _rep.Persistir(aluno);
                ViewBag.Mensagem = "Cadastrado!";
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
}