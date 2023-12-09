using VeteranBot.Gateway.Api;
using VeteranBot.Gateway.Api.Rest.Middlewares;
using VeteranBot.Gateway.Application;
using VeteranBot.Gateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogLogging();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();
builder.Services.AddApi(builder.Environment);

var app = builder.Build();

app.UseMiddleware<ExceptionalMiddleware>();

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyHeader();
    corsPolicyBuilder.AllowAnyOrigin();
    corsPolicyBuilder.AllowAnyMethod();
});

app.UseRouting();

if (app.Environment.IsProduction() is false)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();
app.MapControllers();

await app.RunAsync();