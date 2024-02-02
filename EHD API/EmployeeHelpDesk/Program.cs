using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EHD.BAL.Implementations;
using EHD.BAL.Interface;
using EHD.BAL.Templates;
using EHD.DAL.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var keyVaultConfig = builder.Configuration.GetSection("azure");
var keyVaultUrl = keyVaultConfig["keyvaultUrl"];
var clientId = keyVaultConfig["clientId"];
var clientSecret = keyVaultConfig["clientSecret"];
var tenantId = keyVaultConfig["tenantId"];

ClientSecretCredential clientdetails = new ClientSecretCredential(tenantId, clientId, clientSecret);
SecretClient secretClient = new SecretClient(new Uri(keyVaultUrl), clientdetails);

var connectionStringSecretName = keyVaultConfig["secretName"];
var connectionStringSecret = secretClient.GetSecret(connectionStringSecretName);
var connectionString = connectionStringSecret.Value.Value;
//token generation secret key
var secretKeyName = keyVaultConfig["secretkey"];
var secretValue = secretClient.GetSecret(secretKeyName);
var secretKey = secretValue.Value.Value;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    try
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], //(Array because it can have a list of issuers and audience)
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["azure:secretkey"]))
        };
    }
    catch (Exception e)
    {
        throw new Exception(e.Message);
    }
});

builder.Services.AddDbContext<EHDContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IAccount, AccountRepo>();
builder.Services.AddTransient<IMaster, MasterRepo>();
builder.Services.AddTransient<ITicket, TicketRepo>();
builder.Services.AddTransient<IUser, UserRepo>();
builder.Services.AddTransient<IMailTemplate, MailTemplate>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
