using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Churrascaria.Pages.Conta;

public class EntrarModel : PageModel
{
    private readonly IConfiguration _configuration;
    public EntrarModel(IConfiguration configuration) => _configuration = configuration;
    [BindProperty, Required(ErrorMessage = "Informe o usuário.")]
    public string Usuario { get; set; } = "";
    [BindProperty, Required(ErrorMessage = "Informe a senha.")]
    public string Senha { get; set; } = "";

    public IActionResult OnGet() => User.Identity?.IsAuthenticated == true
        ? RedirectToPage("/Admin/Index") : Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var esperado = _configuration["Admin:Senha"];
        bool senhaCorreta = !string.IsNullOrEmpty(esperado)
            && CryptographicOperations.FixedTimeEquals(
                SHA256.HashData(Encoding.UTF8.GetBytes(Senha)),
                SHA256.HashData(Encoding.UTF8.GetBytes(esperado)));
        if (!senhaCorreta || Usuario != _configuration["Admin:Usuario"])
        {
            ModelState.AddModelError("", "Não foi possível entrar. Verifique seu usuário e sua senha.");
            return Page();
        }
        var identidade = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, Usuario) },
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidade));
        return RedirectToPage("/Admin/Index");
    }
}
