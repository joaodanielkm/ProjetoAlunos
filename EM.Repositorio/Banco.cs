using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace EM.Repository;

public static class Banco
{
    private static FbConnection ObtenhaConexao()
    {
        string diretorio = Directory.GetCurrentDirectory();

        IConfigurationRoot configuracao = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string stringConexao = configuracao.GetConnectionString("StrConexao")!.Replace("@diretorio", diretorio);

        FbConnection conexao = new (stringConexao);
        conexao.Open();

        return conexao;
    }

    public static FbConnection CrieConexao() => ObtenhaConexao();

}


