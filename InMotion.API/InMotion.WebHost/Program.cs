using InMotion.Data.Data;
using InMotion.Data.Models.Auth;
using InMotion.Services;
using InMotion.WebHost.JsonConfiguration;
using InMotion.WebHost.Models;
using InMotion.WebHost.SwaggerConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!, o =>
   {
       o.MigrationsAssembly("InMotion.WebHost");
       o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
   }));

// For Identity
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        /*options.SignIn.RequireConfirmedEmail = true;*/
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddLogging(conf =>
    {
        conf.ClearProviders();

        // conf.AddSeq(configuration.GetSection("Seq"));
        //conf.AddAzureWebAppDiagnostics();
        conf.AddConsole();
    });
/*
    .Configure<AzureFileLoggerOptions>(options =>
    {
        options.FileName = "first-azure-log";
        options.FileSizeLimit = 50 * 1024;
        options.RetainedFileCountLimit = 10;
    });*/

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "http://mapit.studio", "https://mapit.studio", "http://www.mapit.studio", "https://www.mapit.studio", "https://ashy-cliff-062a3df03.2.azurestaticapps.net", "http://ashy-cliff-062a3df03.2.azurestaticapps.net")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddServices();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();