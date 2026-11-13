
using Churrascaria.Models;

namespace Churrascaria.Services;

public class EmailService : IEmailService
{
    public Task NotificarNovoAgendamentoAsync(
        Churrascaria.Models.Agendamento pedido)
    {
        // Implementaremos o envio de e-mails aqui.
        return Task.CompletedTask;
    }
}
