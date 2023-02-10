using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


using AuthenticationServer.JwtSettingsParameters;
using AuthenticationServer.Repositories;
using AuthenticationServer.services;
using AuthenticationServer.ValidationParametersFactory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

JwtSettings jwtSettings = new JwtSettings();

var config = builder.Configuration;
config.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);
string connectionString = config.GetConnectionString("AuthenticationServer");
Console.WriteLine(connectionString);


Console.WriteLine(jwtSettings.AccessTokenSecret);

//ConfigurationBinder.Bind(config, jwtSettings);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthenticationDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("AuthenticationServer")));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();

//TokenValidationParameters tokenValidationParameters = new ValidationParametersFactory(jwtSettings).AccessTokenValidationParameters;
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata= false;
//        options.SaveToken = true;
//        options.TokenValidationParameters = tokenValidationParameters;
//    });

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
