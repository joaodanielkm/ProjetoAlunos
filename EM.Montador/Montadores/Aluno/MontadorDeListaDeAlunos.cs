using EM.Repository;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Montador.Montadores.Aluno;

public class MontadorDeListaDeAlunos : MontadorDePdfAbstrato
{
    protected override string TituloRelatorio => "Lista de Alunos";

    private readonly List<Dominio.Entidades.Aluno> _alunos = [];

    public MontadorDeListaDeAlunos() => _alunos = new RepositorioAluno().GetAll().ToList();

    protected override void MonteCorpoRelatorio()
    {
        PdfPTable tabela = new([15, 25, 60])
        {
            WidthPercentage = 100
        };

        tabela.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        tabela.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        tabela.AddCell("Matricula");
        tabela.AddCell("CPF");
        tabela.AddCell("Nome");

        if (_alunos.Count == 0)
        {
            throw new Exception("Não existem alunos cadastrados!");
        }

        foreach (Dominio.Entidades.Aluno aluno in _alunos)
        {
            tabela.AddCell(aluno.Matricula.ToString());
            tabela.AddCell(aluno.CPF ?? string.Empty);
            tabela.AddCell(new PdfPCell(new Phrase(aluno.Nome)) { HorizontalAlignment = Element.ALIGN_LEFT });
        }

        Documento.Add(tabela);
    }
}
