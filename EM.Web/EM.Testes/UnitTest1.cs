using EM.Dominio.Entidades;
using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using Moq;

namespace EM.Testes;

public class UnitTest1
{
    private readonly Aluno AlunoTeste = new()
    {
        Matricula = 10,
        Nome = "Jose",
        CPF = "652.930.330-85",
        Nascimento = new DateTime(1989, 10, 10),
        Sexo = EnumeradorSexo.Masculino
    };

    [Fact]
    public void Deve_Cadastrar_Aluno_Com_Dados_Validos()
    {
        //Arrange
        var mock = new Mock<IRepositorioAluno>();

        //Act
        mock.Object.Adicione(AlunoTeste);

        //Assert
        mock.Verify(m => m.Adicione(It.Is<Aluno>(
            a => a.Matricula == AlunoTeste.Matricula
            && a.Nome == AlunoTeste.Nome
            && a.CPF == AlunoTeste.CPF
            && Equals(a.Nascimento, AlunoTeste.Nascimento)
            && Equals(a.Sexo, AlunoTeste.Sexo)
            )), Times.Once);
    }
}