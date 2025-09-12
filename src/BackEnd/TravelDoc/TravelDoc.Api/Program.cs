using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Text.Json.Serialization;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Application.Features.Usuarios.Inserir;
using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Events.Messages;
using TravelDoc.Infrastructure.Core.Events;
using TravelDoc.Repository.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddDbContext<TravelDocDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpoints(typeof(InserirUsuarioEndpoint).Assembly);

builder.Services.Scan(scan => scan
                          .FromAssemblies(typeof(InserirUsuarioEndpoint).Assembly)
                          .AddClasses(classes => classes
                            .Where(type => type.GetInterfaces().Any(itf =>
                                   itf != typeof(IAggregateRoot)
                                && type.BaseType != typeof(IntegrationEvent)
                                && type.BaseType != typeof(Entity)
                            )))
                          .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                          .AsImplementedInterfaces()
                          .WithScopedLifetime());

builder.Services.AddMediatR((config) => {
    config.RegisterServicesFromAssemblyContaining<Program>();
    config.RegisterServicesFromAssembly(typeof(InserirUsuarioRequestHandler).Assembly);
});

var app = builder.Build();

app.MapEndpoints();

app.Run();

[JsonSerializable(typeof(InserirUsuarioRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
