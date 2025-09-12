using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Features.Usuarios.Inserir;
using TravelDoc.Repository.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddDbContext<TravelDocDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpoints(typeof(InserirUsuarioEndpoint).Assembly);

builder.Services.AddMediatR((config) => {
    config.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

app.MapEndpoints();

app.Run();

[JsonSerializable(typeof(InserirUsuarioRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
