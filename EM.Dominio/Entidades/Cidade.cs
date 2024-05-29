using EM.Dominio.Enumeradores;

namespace EM.Dominio.Entidades;

public class Cidade
{
    public Cidade(EnumeradorDeEstadoBrasileiro estado) => Estado = estado;

    public int Codigo { get; set; }

    public string Descricao { get; set; }

    public int CodigoIBGE { get; set; }

    public EnumeradorDeEstadoBrasileiro Estado { get; set; }

    public override string ToString() => $"{Descricao} - {EstadoBrasileiro.ObtenhaSigla(Estado.ToString())}";
}
