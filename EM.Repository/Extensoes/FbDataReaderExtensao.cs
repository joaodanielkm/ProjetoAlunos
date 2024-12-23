using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Text;

namespace EM.Repository.Extensoes;

public static class FbDataReaderExtensao
{
    public static EnumeradorSexo GetSexo(this FbDataReader dr, string campo)
    {
        string value = dr.GetString(campo);

        return Enum.TryParse(value, true, out EnumeradorSexo enumerador) ? enumerador : new();
    }

    public static string GetCPF(this FbDataReader dr, string campo)
    {
        string value = dr.GetString(campo);

        return new CPF(value).CPFNumero;
    }
}
