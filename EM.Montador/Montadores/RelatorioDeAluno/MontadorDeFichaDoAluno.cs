using EM.Dominio.Entidades;
using EM.Dominio.Interfaces;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Montador.Montadores.RelatorioDeAluno;

public class MontadorDeFichaDoAluno(string id, IRepositorioAluno repositorioAluno) : MontadorDePdfAbstrato
{
    protected override string TituloRelatorio => "Ficha do Aluno";

    private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;

    protected override void MonteCorpoRelatorio()
    {
        Aluno aluno = _repositorioAluno.Obtenha(id);

        PdfPTable tabelaAluno = new([15, 35, 15, 35])
        {
            WidthPercentage = 100
        };

        tabelaAluno.AddCell(new Phrase("Matricula"));
        tabelaAluno.AddCell(new Phrase(nameof(aluno.Matricula)));

        tabelaAluno.AddCell(new Phrase("CPF"));
        tabelaAluno.AddCell(new Phrase(nameof(aluno.CPF)));

        tabelaAluno.AddCell(new Phrase("Nome"));
        tabelaAluno.AddCell(new Phrase(aluno.Nome));

        tabelaAluno.AddCell(new Phrase("Sexo"));
        tabelaAluno.AddCell(new Phrase(nameof(aluno.Sexo)));

        tabelaAluno.AddCell(new Phrase("Nascimento"));
        tabelaAluno.AddCell(new Phrase(aluno.Nascimento.ToString("dd/MM/YYYY")));

        Documento.Add(tabelaAluno);
    }
}
