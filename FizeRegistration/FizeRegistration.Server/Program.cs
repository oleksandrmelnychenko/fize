
using FizeRegistration.Domain.DbConnectionFactory;
using FizeRegistration.Domain.DbConnectionFactory.Contracts;
using FizeRegistration.Domain.Repositories.Identity;
using FizeRegistration.Domain.Repositories.Identity.Contracts;
using FizeRegistration.Services.IdentityServices;
using FizeRegistration.Services.MailSenderServices.Contracts;
using FizeRegistration.Services.MailSenderServices;
using FizeRegistration.Services.IdentityServices.Contracts;
using Microsoft.AspNetCore.ResponseCompression;

using ConfigManager = FizeRegistration.Common.ConfigurationManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FizeRegistration.Common.IdentityConfiguration;
using Microsoft.Extensions.Primitives;
using FizeRegistration.Shared.ResponseBuilder.Contracts;
using FizeRegistration.Shared.ResponseBuilder;
using FizeRegistration.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);

ConfigManager.SetAppSettingsProperties(builder.Configuration);
NoltFolderManager.InitializeFolderManager(builder.Environment.ContentRootPath);
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IMailSenderFactory, MailSenderFactory>();
builder.Services.AddScoped<IIdentityRepositoriesFactory, IdentityRepositoriesFactory>();
builder.Services.AddScoped<IDetailsRepositoriesFactory, DetatailsRepositoryFactory>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


SymmetricSecurityKey signingKey = AuthOptions.GetSymmetricSecurityKey(ConfigManager.AppSettings.TokenSecret);

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,

    ValidateIssuer = true,
    ValidIssuer = AuthOptions.ISSUER,

    ValidateAudience = true,
    ValidAudience = AuthOptions.AUDIENCE_LOCAL,

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;

    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            StringValues accessToken;
            if (context.Request.Query.ContainsKey("access_token"))
            {
                accessToken = context.Request.Query["access_token"];

                context.Request.Headers.TryAdd("Authorization", $@"Bearer {accessToken}");
            }
            else
            {
                context.Request.Headers.TryGetValue("Authorization", out accessToken);
            }
            return Task.CompletedTask;
        }
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
