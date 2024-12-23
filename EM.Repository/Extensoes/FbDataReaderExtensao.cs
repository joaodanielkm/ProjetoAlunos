using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Text;

namespace EM.Repository.Extensoes;

public static class FbDataReaderExtensao
{
    public static EnumeradorSexo GetSexo(this FbDataReader dr, string campo)//refatorar para uso de generics, posso precisar adicionar outros enumeradores
    {
        string value = dr.GetString(campo);

        if (Enum.IsDefined(typeof(EnumeradorSexo), value))
        {
            return (EnumeradorSexo)Enum.ToObject(typeof(EnumeradorSexo), value);
        }

        return new();
    }

    public static CPF GetCPF(this FbDataReader dr, string campo)
    {
        string value = dr.GetString(campo);

        return new CPF(value);
    }
}
