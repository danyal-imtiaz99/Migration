using migration.Models;
using migration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Services

builder.Services.AddControllers();
builder.Services.AddScoped<CsvService>();
builder.Services.AddScoped<TemplateService>();


// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
    policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure pipeline
app.UseCors("AllowReact");
app.UseRouting();
app.MapControllers();

app.MapGet("/", () => "Migration API is running!");

app.Run();