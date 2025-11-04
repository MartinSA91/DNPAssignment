using FileRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RepositoryContract;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load JWT config
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 🔐 JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// 🧩 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:5100")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 🗂 Repository injection
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors("AllowLocalBlazor");

// ✅ Order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();