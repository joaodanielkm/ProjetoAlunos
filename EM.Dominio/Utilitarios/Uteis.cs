namespace EM.Dominio.Utilitarios;

public static class Uteis
{
    private readonly static DateTime DataNaoInformada = new(1910, 1, 1);

    public static string ApenasNumeros(string texto)
    {
        if (!string.IsNullOrEmpty(texto))
        {
            string textoLimpo = new(texto.Where(char.IsDigit).ToArray());
            return !(!string.IsNullOrWhiteSpace(textoLimpo) && !string.IsNullOrEmpty(textoLimpo)) ? texto : textoLimpo;
        }
        return texto;
    }

    public static DateTime ConvertaData(DateTime? dataEntrada)
    {
        if (Equals(DBNull.Value, dataEntrada)) return DataNaoInformada;

        string dtEntrada = nameof(dataEntrada);
        if (DateTime.TryParse(dtEntrada, out var dt))
        {
            return dt;
        }

        if (!int.TryParse(dtEntrada, out int anoMesDia)) return DataNaoInformada;
        try
        {
            int ano = Convert.ToInt32(anoMesDia.ToString()[..4]);
            int mes = Convert.ToInt32(anoMesDia.ToString().Substring(4, 2));
            int dia = Convert.ToInt32(anoMesDia.ToString().Substring(6, 2));

            return new DateTime(ano, mes, dia);
        }
        catch
        {
            return DataNaoInformada.AddDays(anoMesDia - 2);
        }
    }

    public static bool EhValidoNome(string nome) => nome.Length > 2 && nome.Length < 101;
}
