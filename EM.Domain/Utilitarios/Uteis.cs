namespace EM.Domain.Utilitarios
{
    public static class Uteis
    {
        private readonly static DateTime DataNaoInformada = new (1910, 1, 1);
        public static bool EhValidoCPF(string? cpf)
        {
            if (cpf == null) { return false; }
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
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
                string textoLimpo = new string(texto.Where(char.IsDigit).ToArray());
                if (string.IsNullOrWhiteSpace(textoLimpo) || string.IsNullOrEmpty(textoLimpo))
                {
                    return texto;
                }
                return textoLimpo;
            }
            return texto;
        }

        public static DateTime ConvertaData(DateTime? dataEntrada)
        {
            if (DBNull.Value.Equals(dataEntrada)) return DataNaoInformada;

            var dtEntrada = dataEntrada.ToString();
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

        public static bool EhValidoNome(string nome)
        {
            if (nome.Length < 3 || nome.Length > 100)
            {
                return false;
            }
            return true;
        }
    }
}
