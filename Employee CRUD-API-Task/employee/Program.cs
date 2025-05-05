using employee.Models;
using employee.Repositries.IModelRepositry;
using employee.Repositries.ModelRepositry;
using employee.Services.IModelService;
using employee.Services.ModelService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<RentSaaSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

// Register Repositry
builder.Services.AddScoped<IEmployeeRepositry, EmployeeRepositry>();

// Register Service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
                   .WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
