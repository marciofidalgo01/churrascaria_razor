using System.Globalization;
using System.Threading.RateLimiting;
using Churrascaria.Data;
using Churrascaria.Models;
using Churrascaria.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options => options.Conventions.AuthorizeFolder("/Admin"));
builder.Services.Configure<EmpresaOptions>(builder.Configuration.GetSection("Empresa"));
builder.Services.AddSingleton<RelogioEvento>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<ChurrascariaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Churrascaria")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Conta/Entrar";
        options.AccessDeniedPath = "/Conta/Entrar";
        options.Cookie.Name = "MestreDaBrasa.Equipe";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

// Limita tentativas de acesso e envios repetidos, sem afetar a navegação normal.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        bool login = context.Request.Path.StartsWithSegments("/Conta/Entrar");
        bool pedido = context.Request.Path.StartsWithSegments("/Agendamento/Novo");
        if (!HttpMethods.IsPost(context.Request.Method) || (!login && !pedido))
            return RateLimitPartition.GetNoLimiter("navegacao");

        var chave = $"{context.Connection.RemoteIpAddress}:{(login ? "login" : "pedido")}";
        return RateLimitPartition.GetFixedWindowLimiter(chave, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = login ? 5 : 10,
            Window = TimeSpan.FromMinutes(5),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "text/plain; charset=utf-8";
        await context.HttpContext.Response.WriteAsync(
            "Muitas tentativas seguidas. Aguarde alguns minutos e tente novamente.", token);
    };
});

var app = builder.Build();

var cultura = new CultureInfo("pt-BR");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Projeto novo: cria/aplica somente as migrations incluídas, sem apagar registros.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChurrascariaContext>();
    db.Database.Migrate();
}

app.Run();
