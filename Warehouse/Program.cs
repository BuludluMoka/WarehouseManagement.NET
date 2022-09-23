using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Warehouse.Core.Validator.Products;
using Warehouse.Data.Models;
using Microsoft.Extensions.Configuration;
using Warehouse.Core.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddDbContext<WarehouseDbContext>(options => options.UseSqlServer(ConfigurationDb.ConnectionString));


builder.Services.AddCors(x =>
                x.AddPolicy("AllowAll", x =>
                {
                    x.AllowAnyOrigin();
                    x.AllowAnyMethod();
                    x.AllowAnyHeader();
                })
            );

builder.Services.AddControllers()
    //.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.Preserve)
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//sif (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
//}

app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//