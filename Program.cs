using Agrosphere.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LifePetDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Agrosphere",
        Version = "v1",
        Description = "Tecnologia inspirada em monitoramento espacial aplicada ao agronegócio brasileiro."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Agrosphere v1");
        options.RoutePrefix = "swagger";
        options.InjectStylesheet("/swagger-custom-theme.css");
    });
}
else
{
    app.UseHttpsRedirection();
}

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapControllers();

app.Run();
