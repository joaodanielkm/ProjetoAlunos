using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EM.Montador.Montador.Aluno
{
    public class MontadorDeRelatorioDoAluno : MontadorDePdfAbstrato
    {
        protected override void MonteCorpoRelatorio()
        {
            PdfPTable tabela = new PdfPTable(1)
            {
                WidthPercentage = 100
            };

            PdfPCell celula = new PdfPCell(new Phrase("Teste de relatorio"));

            tabela.AddCell(celula);

            Documento.Add(tabela);
        }
    }
}
