namespace EM.Dominio.Enumeradores;

public enum EnumeradorDeEstadoBrasileiro
{
    ACRE = 1,
    ALAGOAS = 2,
    AMAPA = 3,
    AMAZONAS = 4,
    BAHIA = 5,
    CEARA = 6,
    DISTRITO_FEDERAL = 7,
    ESPIRITO_SANTO = 8,
    GOIAS = 9,
    MARANHAO = 10,
    MATO_GROSSO = 11,
    MATO_GROSSO_DO_SUL = 12,
    MINAS_GERAIS = 13,
    PARA = 14,
    PARAIBA = 15,
    PARANA = 16,
    PERNAMBUCO = 17,
    PIAUI = 18,
    RIO_DE_JANEIRO = 19,
    RIO_GRANDE_DO_NORTE = 20,
    RIO_GRANDE_DO_SUL = 21,
    RONDONIA = 22,
    RORAIMA = 23,
    SANTA_CATARINA = 24,
    SAO_PAULO = 25,
    SERGIPE = 26,
    TOCANTINS = 27
}

public class EstadoBrasileiro
{
    public static string ObtenhaSigla(string estado)
    {
        if (Enum.TryParse(estado.ToUpper(), out EnumeradorDeEstadoBrasileiro enumEstado))
        {
            return enumEstado switch
            {
                EnumeradorDeEstadoBrasileiro.ACRE => "AC",
                EnumeradorDeEstadoBrasileiro.ALAGOAS => "AL",
                EnumeradorDeEstadoBrasileiro.AMAPA => "AP",
                EnumeradorDeEstadoBrasileiro.AMAZONAS => "AM",
                EnumeradorDeEstadoBrasileiro.BAHIA => "BA",
                EnumeradorDeEstadoBrasileiro.CEARA => "CE",
                EnumeradorDeEstadoBrasileiro.DISTRITO_FEDERAL => "DF",
                EnumeradorDeEstadoBrasileiro.ESPIRITO_SANTO => "ES",
                EnumeradorDeEstadoBrasileiro.GOIAS => "GO",
                EnumeradorDeEstadoBrasileiro.MARANHAO => "MA",
                EnumeradorDeEstadoBrasileiro.MATO_GROSSO => "MT",
                EnumeradorDeEstadoBrasileiro.MATO_GROSSO_DO_SUL => "MS",
                EnumeradorDeEstadoBrasileiro.MINAS_GERAIS => "MG",
                EnumeradorDeEstadoBrasileiro.PARA => "PA",
                EnumeradorDeEstadoBrasileiro.PARAIBA => "PB",
                EnumeradorDeEstadoBrasileiro.PARANA => "PR",
                EnumeradorDeEstadoBrasileiro.PERNAMBUCO => "PE",
                EnumeradorDeEstadoBrasileiro.PIAUI => "PI",
                EnumeradorDeEstadoBrasileiro.RIO_DE_JANEIRO => "RJ",
                EnumeradorDeEstadoBrasileiro.RIO_GRANDE_DO_NORTE => "RN",
                EnumeradorDeEstadoBrasileiro.RIO_GRANDE_DO_SUL => "RS",
                EnumeradorDeEstadoBrasileiro.RONDONIA => "RO",
                EnumeradorDeEstadoBrasileiro.RORAIMA => "RR",
                EnumeradorDeEstadoBrasileiro.SANTA_CATARINA => "SC",
                EnumeradorDeEstadoBrasileiro.SAO_PAULO => "SP",
                EnumeradorDeEstadoBrasileiro.SERGIPE => "SE",
                EnumeradorDeEstadoBrasileiro.TOCANTINS => "TO",
                _ => string.Empty,
            };
        }
        return string.Empty;
    }
}
