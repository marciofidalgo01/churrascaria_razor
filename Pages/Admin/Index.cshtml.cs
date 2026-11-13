using Churrascaria.Data;
using Churrascaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Churrascaria.Pages.Admin;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class IndexModel : PageModel
{
    private readonly ChurrascariaContext _context;
    public IndexModel(ChurrascariaContext context) => _context = context;
    [BindProperty(SupportsGet = true)] public DateTime? Data { get; set; }
    [BindProperty(SupportsGet = true)] public StatusAgendamento? Status { get; set; }
    [BindProperty(SupportsGet = true)] public int Pagina { get; set; } = 1;
    [TempData] public string? Mensagem { get; set; }
    public List<Churrascaria.Models.Agendamento> Agendamentos { get; set; } = new();
    public int Total { get; set; }
    public int Pendentes { get; set; }
    public int TotalPaginas { get; set; }

    public async Task OnGetAsync()
    {
        if (!ModelState.IsValid)
        {
            Data = null;
            Status = null;
        }
        var query = _context.Agendamentos.AsNoTracking();
        Pendentes = await query.CountAsync(x => x.Status == StatusAgendamento.Solicitado);
        if (Data.HasValue)
        {
            var inicio = Data.Value.Date;
            var fim = inicio.AddDays(1);
            query = query.Where(x => x.DataHora >= inicio && x.DataHora < fim);
        }
        if (Status.HasValue) query = query.Where(x => x.Status == Status.Value);
        Total = await query.CountAsync();
        TotalPaginas = Math.Max(1, (int)Math.Ceiling(Total / 20d));
        Pagina = Math.Clamp(Pagina, 1, TotalPaginas);
        Agendamentos = await query.OrderBy(x => x.DataHora).ThenBy(x => x.Id)
            .Skip((Pagina - 1) * 20).Take(20).ToListAsync();
    }

    public async Task<IActionResult> OnPostStatusAsync(int id, StatusAgendamento novoStatus)
    {
        if (!ModelState.IsValid || !Enum.IsDefined(novoStatus)) return BadRequest();
        var pedido = await _context.Agendamentos.FindAsync(id);
        if (pedido is null) return NotFound();
        bool transicaoValida = pedido.Status switch
        {
            StatusAgendamento.Solicitado => novoStatus is StatusAgendamento.Confirmado or StatusAgendamento.Cancelado,
            StatusAgendamento.Confirmado => novoStatus is StatusAgendamento.Concluido or StatusAgendamento.Cancelado,
            _ => false
        };
        if (!transicaoValida) return BadRequest();
        pedido.Status = novoStatus;
        await _context.SaveChangesAsync();
        Mensagem = $"Pedido {pedido.Protocolo}: status atualizado para {novoStatus.ToString().ToLowerInvariant()}.";
        return RedirectToPage(new { Data = Data?.ToString("yyyy-MM-dd"), Status, Pagina });
    }
}
