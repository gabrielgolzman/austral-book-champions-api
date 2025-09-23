using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        Description = "Aqu� poner el token generado al loguearse."
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


builder.Services.AddAuthentication("Bearer") 
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"] ?? ""))
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
builder.Services.AddScoped<AuthorService>();
builder.Services.Configure<AuthenticationsServiceOptions>(
builder.Configuration.GetSection(AuthenticationsServiceOptions.AuthenticationService));
builder.Services.AddScoped<ICustomAuthenticationService,AuthenticationService>();

#endregion


var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Enable Swagger in production for debugging
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
