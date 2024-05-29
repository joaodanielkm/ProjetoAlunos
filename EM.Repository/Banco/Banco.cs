using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace EM.Repository;

public static class Banco
{
    public static FbConnection ObtenhaConexao()
    {
        var diretorio = Directory.GetCurrentDirectory();
        string connString = $@"User=SYSDBA;Password=masterkey;Database=localhost/3054:{diretorio}\Banco\DBPROJETOEM.FB4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";

        return new FbConnection(connString);
    }

    public static DataTable Comando(string sql)
    {
        DataTable dt = new ();
        try
        {
            using FbConnection conexaoFireBird = ObtenhaConexao();
            conexaoFireBird.Open();
            using FbDataAdapter da = new(sql, conexaoFireBird);
            da.Fill(dt);
        }
        catch (FbException ex)
        {

            throw new Exception($"Comando {sql}.\nErro do comando: {ex.Message}");
        }

        return dt;
    }
}


