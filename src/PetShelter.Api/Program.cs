using Scalar.AspNetCore;
using PetShelter.Infrastructure;
using PetShelter.Application;
using PetShelter.Api;

var builder = WebApplication.CreateBuilder(args);

await builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructureAsync(builder.Configuration);
    
var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();