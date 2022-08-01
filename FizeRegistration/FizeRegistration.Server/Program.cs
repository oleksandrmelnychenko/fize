using FizeRegistration.Common.ResponseBuilder;
using FizeRegistration.Common.ResponseBuilder.Contracts;
using FizeRegistration.DataBases;
using FizeRegistration.Domain.DbConnectionFactory;
using FizeRegistration.Domain.DbConnectionFactory.Contracts;
using FizeRegistration.Domain.Repositories.Identity;
using FizeRegistration.Domain.Repositories.Identity.Contracts;
using FizeRegistration.Services.IdentityServices;
using FizeRegistration.Services.IdentityServices.Contracts;
using Microsoft.AspNetCore.ResponseCompression;

using ConfigManager = FizeRegistration.Common.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

ConfigManager.SetAppSettingsProperties(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IIdentityRepositoriesFactory, IdentityRepositoriesFactory>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
