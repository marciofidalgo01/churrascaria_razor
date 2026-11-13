using System.ComponentModel.DataAnnotations;

namespace Churrascaria.Models;

public class Agendamento
{
    public int Id { get; set; }
    [MaxLength(12)] public string Protocolo { get; set; } = "";
    [MaxLength(100)] public string NomeCliente { get; set; } = "";
    [MaxLength(20)] public string Telefone { get; set; } = "";
    public DateTime DataHora { get; set; }
    public int QuantidadePessoas { get; set; }
    [MaxLength(40)] public string TipoEvento { get; set; } = "";
    [MaxLength(100)] public string Cidade { get; set; } = "";
    [MaxLength(200)] public string LocalEvento { get; set; } = "";
    [MaxLength(1500)] public string? Observacoes { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Solicitado;
    public DateTime CriadoEmUtc { get; set; } = DateTime.UtcNow;

    [StringLength(254)]
    public string? Email { get; set; }
}

public enum StatusAgendamento { Solicitado, Confirmado, Concluido, Cancelado }
