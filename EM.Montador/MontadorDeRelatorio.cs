using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Montador;

public abstract class MontadorDeRelatorio
{
    public readonly Document Documento = new();

    public byte[] CrieDocumento()
    {
        using MemoryStream memoryStream = new();
        PdfWriter.GetInstance(Documento, memoryStream);
        Documento.Open();
        MonteCabecalhoRelatorio();
        MonteCorpoRelatorio();
        MonteRodaeRelatorio();
        Documento.Close();

        return memoryStream.ToArray();
    }

    protected abstract void MonteCabecalhoRelatorio();

    protected abstract void MonteCorpoRelatorio();

    protected abstract void MonteRodaeRelatorio();
}
