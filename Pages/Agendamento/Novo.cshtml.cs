
using Churrascaria.Data;
using Churrascaria.Models;
using Churrascaria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Churrascaria.Pages.Agendamento;

public class NovoModel : PageModel
{
    private readonly ChurrascariaContext _context;
    private readonly RelogioEvento _relogio;
    private readonly IEmailService _emailService;
    private readonly ILogger<NovoModel> _logger;

    public NovoModel(
        ChurrascariaContext context,
        RelogioEvento relogio,
        IEmailService emailService,
        ILogger<NovoModel> logger)
    {
        _context = context;
        _relogio = relogio;
        _emailService = emailService;
        _logger = logger;
    }

    [BindProperty]
    public DadosAgendamento Agendamento { get; set; } = new();

    [BindProperty]
    public string? Website { get; set; }

    [TempData]
    public string? ProtocoloRecebido { get; set; }

    public string DataMinima =>
        _relogio.Agora.ToString("yyyy-MM-ddTHH:mm");

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(Website))
            return BadRequest();

        if (Agendamento.DataHora.HasValue &&
            Agendamento.DataHora.Value <= _relogio.Agora)
        {
            ModelState.AddModelError(
                "Agendamento.DataHora",
                "Escolha uma data e um horário futuros.");
        }

        if (!DadosAgendamento.TiposEvento.Contains(Agendamento.TipoEvento))
        {
            ModelState.AddModelError(
                "Agendamento.TipoEvento",
                "Escolha um dos tipos de evento da lista.");
        }

        string telefone = string.Concat(
            (Agendamento.Telefone ?? "").Where(char.IsAsciiDigit));

        bool telefoneValido =
            telefone.Length is 10 or 11 ||
            (telefone.StartsWith("55") && telefone.Length is 12 or 13);

        if (!telefoneValido)
        {
            ModelState.AddModelError(
                "Agendamento.Telefone",
                "Informe um telefone brasileiro com DDD.");
        }

        if (!ModelState.IsValid)
            return Page();

        var pedido = new Churrascaria.Models.Agendamento
        {
            Protocolo = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),
            NomeCliente = Agendamento.NomeCliente.Trim(),
            Telefone = telefone,
            Email = string.IsNullOrWhiteSpace(Agendamento.Email)
                ? null
                : Agendamento.Email.Trim(),
            DataHora = Agendamento.DataHora!.Value,
            QuantidadePessoas = Agendamento.QuantidadePessoas!.Value,
            TipoEvento = Agendamento.TipoEvento,
            Cidade = Agendamento.Cidade.Trim(),
            LocalEvento = Agendamento.LocalEvento.Trim(),
            Observacoes = Agendamento.Observacoes?.Trim(),
            Status = StatusAgendamento.Solicitado
        };

        // Primeiro salvamos o pedido no banco.
        _context.Agendamentos.Add(pedido);
        await _context.SaveChangesAsync();

        ProtocoloRecebido = pedido.Protocolo;

        // Depois tentamos enviar os e-mails.
        // Uma falha no envio não deve apagar o agendamento.
        try
        {
            await _emailService.NotificarNovoAgendamentoAsync(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Falha ao enviar notificações do agendamento {Protocolo}",
                pedido.Protocolo);
        }

        // POST → redirect → GET
        return RedirectToPage("./Novo");
    }
}
