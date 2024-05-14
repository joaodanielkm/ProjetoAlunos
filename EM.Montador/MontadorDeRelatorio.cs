using System.IO;
using System;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace EM.Montador
{
    public abstract class MontadorDeRelatorio
    {
        public readonly Document Documento = new Document();

        private readonly static string _diretorioDocumento = $@"{Directory.GetCurrentDirectory()}\DocumentosPDF\";
        private readonly static string _dataAtual = DateTime.Now.ToString("yyyyy-MM-dd-HHmm");
        private static readonly string _formatoDocumento = ".pdf";
        public static string RelatorioNome { get; set; } = _dataAtual;

        public void CrieDocumento()
        {
            CrieFileStream();
            Documento.Open();
            MonteCabecalhoRelatorio();
            MonteCorpoRelatorio();
            MonteRodaeRelatorio();
            Documento.Close();
        }

        protected abstract void MonteCabecalhoRelatorio();

        protected abstract void MonteCorpoRelatorio();

        protected abstract void MonteRodaeRelatorio();

        private string CrieCaminhoENomeDoRelatorio() => Path.Combine(_diretorioDocumento, RelatorioNome + _formatoDocumento);

        private void CrieFileStream()
        {
            if (!Directory.Exists(_diretorioDocumento))
            {
                Directory.CreateDirectory(_diretorioDocumento);
            }

            PdfWriter.GetInstance(Documento, new FileStream(CrieCaminhoENomeDoRelatorio(), FileMode.Create));
        }
    }
}
