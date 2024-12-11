using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<FacturaService>(); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "x-api-key",
        Type = SecuritySchemeType.ApiKey,
        Description = "API Key authentication"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Obtener la API Key del header
        var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();

        // Comparar con la clave configurada en appsettings.json
        if (apiKey != _configuration["ApiKey"])
        {
            // Si no coincide, retornar un error 401 (No autorizado)
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key is missing or invalid");
            return;
        }

        await _next(context);
    }
}
