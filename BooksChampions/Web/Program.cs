using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.Services;
using Infrastructure.Services.Auth0;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using static Infrastructure.Services.AuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("BookChampionsApiBearerAuth", new OpenApiSecurityScheme() 
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Aquí poner el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BookChampionsApiBearerAuth" }
                }, new List<string>() }
    });


});

string connectionString = builder.Configuration["ConnectionStrings:BooksDBConnectionString"]!;

// Configure the SQLite connection
var connection = new SqliteConnection(connectionString);
connection.Open();

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication("Bearer") 
    .AddJwtBearer(options => 
    {
        //options.TokenValidationParameters = new()
        //{
        //    ValidateIssuer = true,
        //    ValidateAudience = true,
        //    ValidateIssuerSigningKey = true,
        //    ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
        //    ValidAudience = builder.Configuration["AuthenticationService:Audience"],
        //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"] ?? ""))
        //};

        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };    
    }
);

builder.Services.AddDbContext<BookDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));

#region Repositories

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();


#endregion

#region Services

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.Configure<AuthenticationsServiceOptions>(
   builder.Configuration.GetSection(AuthenticationsServiceOptions.AuthenticationService));
builder.Services.AddScoped<ICustomAuthenticationService,AuthenticationService>();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
