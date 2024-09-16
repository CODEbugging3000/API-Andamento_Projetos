using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

//builder de autenticação
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = "JwTBearer";
    options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quem está solicitando.
        ValidateIssuer = true,
        // Valida quem está recebendo.
        ValidateAudience = true,
        // Define se o tempo de expiração será validado.
        ValidateLifetime = true,
        // Criptografia e validação da chave de autenticacão.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        ClockSkew = TimeSpan.FromMinutes(30), //Valida o tempo de expitação do token
        ValidIssuer = "exoapi.webapi", //Nome do issuer(origem)
        ValidAudience = "exoapi.webapi" //Nome do audience(Destino)
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
