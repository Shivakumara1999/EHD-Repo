using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EHD.BAL.Implementations;
using EHD.BAL.Interface;
using EHD.BAL.Templates;
using EHD.DAL.DataContext;
using Microsoft.EntityFrameworkCore;

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
