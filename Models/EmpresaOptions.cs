namespace Churrascaria.Models;

public class EmpresaOptions
{
    public string Nome { get; set; } = "Mestre da Brasa";
    public string Descricao { get; set; } = "Churrasco para o seu evento.";
    public string WhatsApp { get; set; } = "";
    public string Instagram { get; set; } = "";
    public string Regiao { get; set; } = "Local de atendimento a combinar";
    public string FotoPrincipal { get; set; } = "";
    public string FotoPrincipalAlt { get; set; } = "Churrasco preparado pela equipe";
    public string FusoHorario { get; set; } = "America/Sao_Paulo";
    public List<FotoPortfolio> Portfolio { get; set; } = new();

    public string? LinkWhatsApp
    {
        get
        {
            string numero = string.Concat(WhatsApp.Where(char.IsAsciiDigit));
            return numero.Length is >= 10 and <= 15
                ? $"https://wa.me/{numero}?text={Uri.EscapeDataString("Olá! Gostaria de um orçamento para meu evento.")}"
                : null;
        }
    }

    public string? LinkInstagram => Uri.TryCreate(Instagram, UriKind.Absolute, out var url)
        && url.Scheme == "https" && (url.Host == "instagram.com" || url.Host == "www.instagram.com")
        ? url.AbsoluteUri : null;
}

public class FotoPortfolio
{
    public string Caminho { get; set; } = "";
    public string Titulo { get; set; } = "";
    public string Categoria { get; set; } = "";
    public string Descricao { get; set; } = "";
}
