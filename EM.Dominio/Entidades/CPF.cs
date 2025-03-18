using EM.Dominio.Utilitarios;

namespace EM.Dominio.Entidades;

public class CPF(string CPFFormatado)
{
    private string Cpf { get; set; } = CPFFormatado;

    public string CPFNumero => Uteis.ApenasNumeros(Cpf);

    public bool EhValidoCPF()
    {
        if (string.IsNullOrEmpty(Cpf)) { return false; }
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf;
        string digito;
        int soma;
        int resto;
        if (string.IsNullOrEmpty(Cpf))
        {
            return false;
        }
        Cpf = Cpf.Trim().Replace(".", "").Replace("-", "");
        if (Cpf.Length != 11)
            return false;
        tempCpf = Cpf[..9];
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
        return Cpf.EndsWith(digito);
    }

    public override string ToString() => Cpf;
}
