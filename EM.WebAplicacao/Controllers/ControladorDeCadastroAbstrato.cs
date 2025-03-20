namespace EM.Web.Controllers
{
    public abstract class ControladorDeCadastroAbstrato<T>
        : ControllerAbstrato
        where T : class, new()
    {
        private static string? DescricaoModel => typeof(T).Name;
        protected string ViewCadastro => $"~/Views/Cadastros/{DescricaoModel}/_Cadastra.cshtml";
        protected string ViewEditar => $"~/Views/Cadastros/{DescricaoModel}/_Edita.cshtml";
        protected virtual void Grave(T model) => throw new NotImplementedException();

    }
}
