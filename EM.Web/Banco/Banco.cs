using System.Data;
using EM.Domain.ProjetoEM.EM.Domain;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Reflection;

namespace EM.Repository.Banco
{
    public class Banco
    {
        Aluno aluno = new Aluno();
        public FbConnection? conexao;
        private static readonly Banco instanciaFireBird = new Banco();


        public static Banco getInstancia()
        {
            return instanciaFireBird;
        }

        public FbConnection getConexao()
        {

            string[] filePaths = Directory.GetFiles(@".\Banco", "DBPROJETOEM-.FD4");
            Console.WriteLine(filePaths);
            //string conn = @"User=SYSDBA;Password=masterkey;Database="+filePaths+";DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";
            //string conn = @"User=SYSDBA;Password=masterkey;Database=192.168.1.160/3054:C:\ProjetoEM\EM.Repository\Banco\\DBPROJETOEM-.FD4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";
            string conn = @"User=SYSDBA;Password=masterkey;Database=C:\ProjetoEM\EM.Repository\Banco\DBPROJETOEM-.FD4;DataSource=localhost;Dialect=3;Charset=NONE;Pooling=true;user=sysdba;password=masterkey;dialect=3;";

            return new FbConnection(conn);
        }

        public static DataTable consulta(string sql)
        {

            FbDataAdapter? da = null;
            DataTable dt = new DataTable();

            try
            {
                using (FbConnection conexaoFireBird = getInstancia().getConexao())
                {
                    conexaoFireBird.Open();
                    da = new FbDataAdapter(sql, conexaoFireBird);
                    da.Fill(dt);
                    conexaoFireBird.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}


