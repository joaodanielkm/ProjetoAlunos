namespace EM.Web.Controllers
{
    public abstract class ControladorDeCadastroAbstrato<T>
        : ControllerAbstrato
        where T : class, new()
    {
        protected virtual string ObterNomeEntidade() => typeof(T).Name.EndsWith("Model")
            ? typeof(T).Name[..^5]
            : typeof(T).Name;

        protected string ViewCadastro => $"~/Views/Cadastros/{ObterNomeEntidade()}/_Cadastra.cshtml";
        protected string ViewEditar => $"~/Views/Cadastros/{ObterNomeEntidade()}/_Edita.cshtml";

    }
}
