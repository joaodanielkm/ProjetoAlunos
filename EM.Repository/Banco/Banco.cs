using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace EM.Repository
{
    public static class Banco
    {
        public static FbConnection ObtenhaConexao()
        {
            //string[] filePaths = Directory.GetFiles(@".\Banco", "DBPROJETOEM-.FD4");
            //Console.WriteLine(filePaths);
            //string conn = @"User=SYSDBA;Password=masterkey;Database="+filePaths+";DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";
            string conn = @"User=SYSDBA;Password=masterkey;Database=192.168.1.217/3054:E:\EscolarManager\Dados\EMWeb\DBPROJETOEM.FB4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";
            //string conn = @"User=SYSDBA;Password=masterkey;Database=C:\ProjetoEM\EM.Repository\Banco\DBPROJETOEM-.FD4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";

            return new FbConnection(conn);
        }

        public static DataTable Consulta(string sql)
        {
            DataTable dt = new();
            try
            {
                using FbConnection conexaoFireBird = ObtenhaConexao();
                conexaoFireBird.Open();
                FbDataAdapter da = new(sql, conexaoFireBird);
                da.Fill(dt);
                conexaoFireBird.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o banco {ex}");
            }
        }
    }
}


