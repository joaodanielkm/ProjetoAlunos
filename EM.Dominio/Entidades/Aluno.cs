﻿using EM.Dominio.Enumeradores;
using EM.Dominio.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EM.Dominio.Entidades;

public class Aluno(string cpf) : IEntidade
{
    [Key]
    [Display(Name = "Matricula")]
    [Required(ErrorMessage = "Matricula Requerida!")]
    [Range(1, 999999999, ErrorMessage = "Matricula invalida!")]
    public int Matricula { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Nome Requerido!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Favor preencher com no minimo 3 e no máximo 100 caracteres!")]
    public string Nome { get; set; }

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "Sexo Requerido!")]
    public EnumeradorSexo Sexo { get; set; }

    [Display(Name = "Nascimento")]
    [Required(ErrorMessage = "Nascimento Requerido!")]
    [DataType(DataType.Date)]
    public DateTime Nascimento { get; set; }

    [Display(Name = "CPF")]
    [MaxLength(14)]
    public CPF CPF { get; set; } = new CPF(cpf);

    public override bool Equals(object obj) =>
        obj is Aluno aluno &&
               Matricula == aluno.Matricula &&
               Nome == aluno.Nome &&
               CPF == aluno.CPF &&
               Nascimento == aluno.Nascimento;

    public override int GetHashCode() => HashCode.Combine(Matricula);

    public override string ToString() => Matricula + " - " + Nome;
}
