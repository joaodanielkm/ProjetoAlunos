using EM.Dominio.Utilitarios;

namespace EM.Dominio.Entidades;

public class CPF(string cpf)
{
    private string _cpfFormatado = cpf;

    public string CPFNumero => Uteis.ApenasNumeros(_cpfFormatado);

    public string CPFFormatado => FormateCPF(CPFNumero);

    public static string FormateCPF(string numero) =>
        $"{numero[..3]}.{numero.Substring(3, 3)}.{numero.Substring(6, 3)}-{numero.Substring(9, 2)}";

    public bool EhValidoCPF()
    {
        if (string.IsNullOrEmpty(_cpfFormatado)) { return false; }
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf;
        string digito;
        int soma;
        int resto;
        if (string.IsNullOrEmpty(_cpfFormatado))
        {
            return false;
        }
        _cpfFormatado = _cpfFormatado.Trim().Replace(".", "").Replace("-", "");
        if (_cpfFormatado.Length != 11)
            return false;
        tempCpf = _cpfFormatado[..9];
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
        return _cpfFormatado.EndsWith(digito);
    }

    public override string ToString() => _cpfFormatado;
}
