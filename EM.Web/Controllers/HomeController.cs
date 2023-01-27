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
                    alunosPorNome = alunosPorNome.Where(a => a.Nome!.Contains(searchString.ToUpper()));
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
        public IActionResult Editar(Aluno getAluno)
        {
            Domain.Utilitarios.Uteis uteis = new Domain.Utilitarios.Uteis();

            var aluno = new Aluno()
            {

                Matricula = getAluno.Matricula,
                Nome = getAluno.Nome.ToUpper(),
                Sexo = getAluno.Sexo,
                Nascimento = getAluno.Nascimento,
                CPF = (uteis.EhValidoCPF(getAluno.CPF)) ? getAluno.CPF : null,

            };
            try
            {
                _rep.Atualizar(aluno);
                ViewBag.Mensagem = "Atualizado!";
                //return RedirectToAction("Index", "Home");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastrar");
                ViewBag.Mensagem = "erro";
            }
            return View();
        }

        public IActionResult Cadastrar()
        {
            Aluno aluno = new Aluno();

            var getUltimaMatriculaMaisUm = _rep.Listar().Max(a => a.Matricula) + 1;

            aluno.Sexo = Sexo.Masculino;
            aluno.Matricula = getUltimaMatriculaMaisUm;

            return View(aluno);
        }

        [HttpPost]
        public IActionResult Cadastrar(Aluno getAluno)
        {
            var getMatriculas = from a in _rep.Listar()
                                select a;
            var getUltimaMatriculaMaisUm = _rep.Listar().Max(a => a.Matricula) + 1;

            var aluno = new Aluno()
            {
                Matricula = (getAluno.Matricula > 0) ? getAluno.Matricula : getUltimaMatriculaMaisUm,
                Nome = getAluno.Nome.ToUpper(),
                Sexo = getAluno.Sexo,
                Nascimento = getAluno.Nascimento,
                CPF = (uteis.EhValidoCPF(getAluno.CPF)) ? getAluno.CPF : "",
            };
            Aluno? verificaSeMatriculaExiste = _rep.Selecionar(getAluno.Matricula.ToString());

            if (verificaSeMatriculaExiste == null)
            {
                try
                {
                    _rep.Persistir(aluno);
                    ViewBag.Mensagem = "Cadastrado!";
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
                ViewBag.Mensagem = "Matricula já em uso!";
                return View();
            }
            return View();
            // return RedirectToAction("Cadastrar");

        }

        public IActionResult Deletar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                _rep.Excluir(id.ToString());
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