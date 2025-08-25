using EM.Dominio.Enumeradores;
using FirebirdSql.Data.FirebirdClient;

namespace EM.Repositorio.Extensoes;

public static class FbDataReaderExtensao
{
    public static string GetStringSafe(this FbDataReader dr, string campo)
    {
        var ordinal = dr.GetOrdinal(campo);
        return dr.IsDBNull(ordinal) ? string.Empty : dr.GetString(ordinal);
    }

    public static EnumeradorSexo GetSexo(this FbDataReader dr, string campo)
    {
        string value = dr.GetStringSafe(campo);
        return Enum.TryParse(value, true, out EnumeradorSexo enumerador) ? enumerador : EnumeradorSexo.Masculino;
    }
}
