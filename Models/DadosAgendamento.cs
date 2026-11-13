using System.ComponentModel.DataAnnotations;

namespace Churrascaria.Models;

// Apenas campos do formulário: o visitante não pode escolher Id, status ou protocolo.
public class DadosAgendamento
{

    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(254)]
    public string? Email { get; set; }

    public static readonly string[] TiposEvento =
        { "Aniversário", "Casamento", "Confraternização", "Evento corporativo", "Outro" };

    [Required(ErrorMessage = "Informe seu nome.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Use entre 2 e 100 caracteres.")]
    [Display(Name = "Seu nome")]
    public string NomeCliente { get; set; } = "";

    [Required(ErrorMessage = "Informe um telefone para contato.")]
    [StringLength(20, ErrorMessage = "O telefone deve ter até 20 caracteres.")]
    [RegularExpression(@"[0-9+()\s-]+", ErrorMessage = "Informe somente números e a formatação do telefone.")]
    [Display(Name = "Telefone / WhatsApp")]
    public string Telefone { get; set; } = "";

    [Required(ErrorMessage = "Informe a data e o horário do evento.")]
    [Display(Name = "Data e horário")]
    public DateTime? DataHora { get; set; }

    [Required(ErrorMessage = "Informe a quantidade de convidados.")]
    [Range(1, 1000, ErrorMessage = "Informe entre 1 e 1.000 convidados. A equipe avaliará o atendimento.")]
    [Display(Name = "Número de convidados")]
    public int? QuantidadePessoas { get; set; }

    [Required(ErrorMessage = "Escolha o tipo de evento.")]
    [Display(Name = "Tipo de evento")]
    public string TipoEvento { get; set; } = "";

    [Required(ErrorMessage = "Informe a cidade do evento.")]
    [StringLength(100, ErrorMessage = "Use até 100 caracteres.")]
    [Display(Name = "Cidade / bairro")]
    public string Cidade { get; set; } = "";

    [Required(ErrorMessage = "Informe o local, mesmo que provisório.")]
    [StringLength(200, ErrorMessage = "Use até 200 caracteres.")]
    [Display(Name = "Local do evento")]
    public string LocalEvento { get; set; } = "";

    [StringLength(1500, ErrorMessage = "Use até 1.500 caracteres.")]
    [Display(Name = "Conte um pouco sobre o evento")]
    public string? Observacoes { get; set; }

   
}
