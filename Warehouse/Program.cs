using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Warehouse.Core.Validator.Products;
using Warehouse.Data.Models;
using Microsoft.Extensions.Configuration;
using Warehouse.Core.Configuration;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Warehouse.Data.Models.Common.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
//sif (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
//}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


