using EM.Dominio.Utilitarios;
using System.ComponentModel.DataAnnotations;

namespace EM.Dominio.Entidades;

public class CPF
{
    public string Numero { get; }

    public string Formatado => Formate(Numero);

    public CPF(string? numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
        {
            Numero = "";
            return;
        }

        Numero = Uteis.ApenasNumeros(numero);
        if (!EhValido(Numero))
            throw new ValidationException("CPF inválido!");
    }

    public static string Formate(string? numero)
    {
        if (string.IsNullOrWhiteSpace(numero) || numero.Length != 11)
            return numero ?? "";

        return Convert.ToUInt64(numero).ToString(@"000\.000\.000\-00");
    }

    public static bool EhValido(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var cpfNumeros = Uteis.ApenasNumeros(cpf);

        if (cpfNumeros.Length != 11)
            return false;

        if (cpfNumeros.All(c => c == cpfNumeros[0]))
            return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpfNumeros.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cpfNumeros[9].ToString()) != digito1)
            return false;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpfNumeros[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cpfNumeros[10].ToString()) == digito2;
    }

    public override string ToString() => Numero;

    public override bool Equals(object? obj) => obj is CPF outro && Numero == outro.Numero;

    public override int GetHashCode() => Numero.GetHashCode();
}
