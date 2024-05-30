using EM.Montador.Utilidades;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Montador;

public abstract class MontadorDePdfAbstrato : MontadorDeRelatorio
{
    protected virtual string TituloRelatorio => "Relatorio";

    protected override void MonteCabecalhoRelatorio()
    {
        PdfPTable cabecalho = new(1)
        {
            WidthPercentage = 100
        };

        cabecalho.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        cabecalho.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        cabecalho.DefaultCell.Border = Rectangle.NO_BORDER;
        cabecalho.SpacingAfter = 10;

        cabecalho.AddCell(new PdfPCell(new Phrase(TituloRelatorio, Fontes.FonteHelvetica17Negrito))
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 20
        });
        Documento.Add(cabecalho);
    }

    protected override void MonteRodaeRelatorio()
    {
        PdfPTable rodape = new(1)
        {
            WidthPercentage = 100
        };

        rodape.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        rodape.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        rodape.DefaultCell.Border = Rectangle.NO_BORDER;
        rodape.SpacingBefore = 10;

        rodape.AddCell(new PdfPCell(new Phrase("Rodapé", Fontes.FonteHelvetica14Italico))
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            Border = Rectangle.NO_BORDER,
            PaddingTop = 20
        });
        Documento.Add(rodape);
    }
}
