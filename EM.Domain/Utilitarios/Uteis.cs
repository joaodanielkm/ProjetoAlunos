using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EM.Domain.Utilitarios
{
    public class Uteis
    {
        public static DateTime DataNaoInformada = new (1910, 1, 1);
        public bool EhValidoCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            if (String.IsNullOrEmpty(cpf))
            {
                return false;
            }
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        public string ApenasNumeros(string texto)
        {
            if (!String.IsNullOrEmpty(texto))
            {
                string textoLimpo = new string(texto.Where(char.IsDigit).ToArray());
                if (string.IsNullOrWhiteSpace(textoLimpo) || string.IsNullOrEmpty(textoLimpo))
                {
                    return texto;
                }
                return textoLimpo;
            }
            return texto;

        }
        public DateTime ConvertaData(object dataEntrada)
        {
            if (DBNull.Value.Equals(dataEntrada)) return DataNaoInformada;

            var dtEntrada = dataEntrada?.ToString();
            if (DateTime.TryParse(dtEntrada, out var dt))
            {
                return dt;
            }

            if (!int.TryParse(dtEntrada, out int anoMesDia)) return DataNaoInformada;
            try
            {
                var ano = Convert.ToInt32(anoMesDia.ToString().Substring(0, 4));
                var mes = Convert.ToInt32(anoMesDia.ToString().Substring(4, 2));
                var dia = Convert.ToInt32(anoMesDia.ToString().Substring(6, 2));

                return new DateTime(ano, mes, dia);
            }
            catch
            {
                return DataNaoInformada.AddDays(anoMesDia - 2);
            }


        }
        public object DataToBD(DateOnly? Data)
        {
            if (!Data.HasValue) return DBNull.Value;
            return Data.Value.ToString("yyyyMMdd");
        }
    }
}
