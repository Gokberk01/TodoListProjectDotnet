using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Extensions.Logging;
using todoapi.Data;
using todoapi.Repositories;
using todoapi.Services;
using todoapi.Shared;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();



var key = Encoding.ASCII.GetBytes("this is a super rare and very secure key");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//when we create a single page application the browser will prevent request from different ports so our port is 5136 
//and front end will run different port. When the frontend send a request to our backend it will prevent that and to not allow it 
//we have to add cores here
builder.Services.AddCors(); 
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();
builder.Services.AddScoped<IUserService, UserService>();


//Buraya gelicekk
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));


builder.Host.UseSerilog((context, loggerConfiguration) =>
{
loggerConfiguration
.ReadFrom.Configuration(context.Configuration)
.Enrich.FromLogContext()
.Enrich.WithMachineName()
.Enrich.WithEnvironmentName()
.WriteTo.Console();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options //3000 -> REACT , 8080 -> VIEW , 4200 -> ANGULAR We can pick only one
    .WithOrigins(new []{"http://localhost:3000" , "http://localhost:8080", "http://localhost:4200"})
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials() //This is the option that we need in order to send cookies in the 
    //front end if we dont add this option the front end wont get those cookies
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();

