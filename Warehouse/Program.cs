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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}





#region Error Mid

//app.Use(async (context, next) =>
//{
//    var newContent = string.Empty;

//    var existingBody = context.Response.Body;

//    using (var newBody = new MemoryStream())
//    {
//        // We set the response body to our stream so we can read after the chain of middlewares have been called.
//        context.Response.Body = newBody;

//        await next();

//        // Reset the body so nothing from the latter middlewares goes to the output.
//        context.Response.Body = new MemoryStream();

//        newBody.Seek(0, SeekOrigin.Begin);
//        context.Response.Body = existingBody;
//        // newContent will be `Hello`.
//        newContent = new StreamReader(newBody).ReadToEnd();

//        newContent += ", World!";

//        // Send our modified content to the response body.
//        await context.Response.WriteAsync(newContent);
//    }
//});

#endregion



app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();





app.Run();


