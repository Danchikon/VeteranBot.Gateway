using Amazon;
using VeteranBot.Gateway.Api;
using VeteranBot.Gateway.Api.Rest.Middlewares;
using VeteranBot.Gateway.Application;
using VeteranBot.Gateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var host = builder.Host;
var environment = builder.Environment;
var configuration = builder.Configuration;
var services = builder.Services;

host.UseSerilogLogging();

configuration.AddEnvironmentVariables();
configuration.AddAwsConfiguration(configuration, environment);

services.AddInfrastructure(configuration, environment);
services.AddApplication();
services.AddApi(environment);

var app = builder.Build();

// app.Lifetime.ApplicationStarted.Register(() =>
// {
//     Task.WhenAll(
//             app.AwsS3PutBucketsAsync(new [] { FoldersNames.AvatarFolder }, app.Lifetime.ApplicationStopping)
//             )
//         .Wait();
// });

app.UseMiddleware<ExceptionalMiddleware>();

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyHeader();
    corsPolicyBuilder.AllowAnyOrigin();
    corsPolicyBuilder.AllowAnyMethod();
});

app.UseRouting();

if (environment.IsProduction() is false)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();
app.MapControllers();

await app.RunAsync();