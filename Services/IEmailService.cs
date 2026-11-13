using Churrascaria.Models;

namespace Churrascaria.Services;

public interface IEmailService
{
    Task NotificarNovoAgendamentoAsync(
        Churrascaria.Models.Agendamento pedido);
}