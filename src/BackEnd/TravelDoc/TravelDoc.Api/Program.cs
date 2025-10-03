using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Text.Json.Serialization;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Features.Usuarios.Inserir;
using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Events;
using TravelDoc.Repository.Contexts;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// DbContext
builder.Services.AddDbContext<TravelDocDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Endpoints
builder.Services.AddEndpoints(typeof(InserirUsuarioEndpoint).Assembly);

// Registro de serviços (excluindo Requests do MediatR)
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(InserirUsuarioEndpoint).Assembly)
    .AddClasses(classes => classes
        .Where(type =>
            // ignora classes que implementam IRequest<>
            !(type.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)))
            // e ignora as entidades/eventos de domínio
            && type.GetInterfaces().Any(itf =>
                   itf != typeof(IAggregateRoot)
                && type.BaseType != typeof(IntegrationEvent)
                && type.BaseType != typeof(Entity)
            )))
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// MediatR (registra handlers automaticamente)
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
    config.RegisterServicesFromAssembly(typeof(InserirUsuarioRequestHandler).Assembly);
});

// URLs
builder.WebHost.UseUrls("http://0.0.0.0:5005;https://0.0.0.0:5005");

var app = builder.Build();

// Swagger em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapear endpoints
app.MapEndpoints();

app.Run();

// Serialização
[JsonSerializable(typeof(InserirUsuarioRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
