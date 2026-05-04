using Scalar.AspNetCore;
using PetShelter.Infrastructure;
using PetShelter.Application;
using PetShelter.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);
    
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PetShelter.Infrastructure.Persistence.PetShelterDbContext>();
    dbContext.Database.Migrate();
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();