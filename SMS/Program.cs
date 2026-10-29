using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMS.DbContexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,//the source of the token is not validated
            ValidateAudience = false,//the user can use the token in any application
            ValidateIssuerSigningKey = true,//make sure the token is using my secret key 
            IssuerSigningKey = new SymmetricSecurityKey(key)//the secret key used to sign the token
        };
    });

builder.Services.AddDbContext<SchoolContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
