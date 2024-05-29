using iTextSharp5.text.pdf;

namespace EM.Montador.Montador.Aluno;

public class MontadorDeRelatorioDoAluno() : MontadorDePdfAbstrato
{
    private readonly List<Aluno> _alunos = [];

    public void EmitaTodosAlunos()
    {
        //foreach (Aluno aluno in _alunos)
        //{
        //    EmitaAluno(aluno);
        //}

        CrieDocumento();
    }

    public void EmitaAluno( )
    {
        MonteCorpoRelatorio();
    }

    protected override void MonteCorpoRelatorio()
    {
        PdfPTable tabela = new(1)
        {
            WidthPercentage = 100
        };

        PdfPCell celula = new(new iTextSharp5.text.Phrase("teste"));

        tabela.AddCell(celula);

        Documento.Add(tabela);
    }
}
