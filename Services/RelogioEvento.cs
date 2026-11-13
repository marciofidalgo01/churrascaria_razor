using Churrascaria.Models;
using Microsoft.Extensions.Options;

namespace Churrascaria.Services;

public class RelogioEvento
{
    private readonly TimeZoneInfo _fuso;
    public RelogioEvento(IOptions<EmpresaOptions> empresa) =>
        _fuso = TimeZoneInfo.FindSystemTimeZoneById(empresa.Value.FusoHorario);
    public DateTime Agora => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _fuso);
}
