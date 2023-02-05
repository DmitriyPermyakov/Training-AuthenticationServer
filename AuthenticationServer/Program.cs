using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


using AuthenticationServer.JwtSettingsParameters;
using AuthenticationServer.Repositories;
using AuthenticationServer.services;
using AuthenticationServer.ValidationParametersFactory;

var builder = WebApplication.CreateBuilder(args);

JwtSettings jwtSettings = new JwtSettings();
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.Build();
config.Bind("JwtSettings", jwtSettings);


//ConfigurationBinder.Bind(config, jwtSettings);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddSingleton(jwtSettings);

TokenValidationParameters tokenValidationParameters = new ValidationParametersFactory(jwtSettings).AccessTokenValidationParameters;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata= false;
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localHostConnection", builder => builder.WithOrigins("https://localhost:4200")
        .AllowAnyHeader()
        .WithMethods("PUT", "POST", "GET", "DELETE"));
});

var app = builder.Build();

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
