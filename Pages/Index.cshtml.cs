using Churrascaria.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Churrascaria.Pages;

public class IndexModel : PageModel
{
    public EmpresaOptions Empresa { get; }
    public IndexModel(IOptions<EmpresaOptions> empresa) => Empresa = empresa.Value;
    public void OnGet() { }
}
