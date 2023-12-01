using RealworldConduit.Infrastructure.Extensions;
using RealworldConduit.Infrastructure.Filters;
using RealWorldConduit.Infrastructure.Extensions;
using RealWorldConduit.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

// Dependency Injection
builder.Services.ServicesConfiguration()
                .MediatRConfiguration()
                .FluentValidationConfiguration()
                .FluentValidationFilterConfiguration()
                .AuthConfiguration(builder.Configuration)
                .DatabaseConfiguration(builder.Configuration);


var app = builder.Build();

//Middleware usage
app.UseMiddleware<RestExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
