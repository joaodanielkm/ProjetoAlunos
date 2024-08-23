namespace EM.Dominio.Utilitarios;

public static class Uteis
{
    private readonly static DateTime DataNaoInformada = new(1910, 1, 1);

    public static bool EhValidoCPF(string cpf)
    {
        if (cpf == null) { return false; }
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf;
        string digito;
        int soma;
        int resto;
        if (string.IsNullOrEmpty(cpf))
        {
            return false;
        }
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf[..9];
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf += digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito += resto.ToString();
        return cpf.EndsWith(digito);
    }

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
