using EM.Repository;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Montador.Montadores.RelatorioDeAluno;

public class MontadorDeFichaDoAluno(string id) : MontadorDePdfAbstrato
{
    protected override string TituloRelatorio => "Ficha do Aluno";

    private readonly Dominio.Entidades.Aluno _aluno = new RepositorioAluno().Obtenha(id);

    protected override void MonteCorpoRelatorio()
    {
        PdfPTable tabelaAluno = new([15, 35, 15, 35])
        {
            WidthPercentage = 100
        };

        tabelaAluno.AddCell(new Phrase("Matricula"));
        tabelaAluno.AddCell(new Phrase(nameof(_aluno.Matricula)));

        tabelaAluno.AddCell(new Phrase("CPF"));
        tabelaAluno.AddCell(new Phrase(nameof(_aluno.CPF)));

        tabelaAluno.AddCell(new Phrase("Nome"));
        tabelaAluno.AddCell(new Phrase(_aluno.Nome));

        tabelaAluno.AddCell(new Phrase("Sexo"));
        tabelaAluno.AddCell(new Phrase(nameof(_aluno.Sexo)));

        tabelaAluno.AddCell(new Phrase("Nascimento"));
        tabelaAluno.AddCell(new Phrase(_aluno.Nascimento.ToString("dd/MM/YYYY")));

        Documento.Add(tabelaAluno);
    }
}
