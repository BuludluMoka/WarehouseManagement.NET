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
using Warehouse.Core.Services.EmailService;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);



var EmailConf=  builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddControllers()
               .ConfigureApiBehaviorOptions(options =>
               {
                   options.SuppressMapClientErrors = true;
               })
               .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());
builder.Services.AddServices();
builder.Services.AddSingleton(EmailConf);
builder.Services.AddScoped<IEmailSender, EmailSender>();



builder.Services.Configure<FormOptions>(o => {
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
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


//app.Use(async (contex, next) =>
//{

//});


app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();





app.Run();


