using Microsoft.AspNetCore.Mvc;
using Shop.API.ApiModel.Response;
using Shop.API.ExceptionHandler;
using Shop.API.Services;
using Shop.Core.IServices;
using Shop.Infrastructure.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

RegisterServices(builder.Services);

AddApiVersioningConfigured(builder.Services);

void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IService, HttpService>();
    services.AddScoped<ICustomerService<ApiResponse>, CustomerService>();
    services.AddScoped<IProductService<ApiResponse>, ProductService>();
}

void AddApiVersioningConfigured(IServiceCollection services)
{
    services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);

    });

}

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
