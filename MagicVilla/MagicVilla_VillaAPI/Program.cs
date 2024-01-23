//disabled nullable is csproj
// SERILOG using Serilog;
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//added support for newstonsoftjoson

builder.Services.AddDbContext<ApplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    });

builder.Services.AddResponseCaching();

// SERILOG Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();

// SERILOG builder.Host.UseSerilog();

builder.Services.AddControllers(option=>
{
    //turning this true requires adding serializer formaters for every type
    option.ReturnHttpNotAcceptable = false;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


//builder.Services.AddEndpointsApiExplorer(); // probably only needed for minimal apis

/*
builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(2, 0);
});
sdfsdaf
*/

builder.Services.AddApiVersioning(options =>
{

    options.ReportApiVersions = true;
    options.UnsupportedApiVersionStatusCode = 404; ///;// (StatusCodes.Status404NotFound);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
})
    .AddMvc()
    .AddApiExplorer(options =>
{
   options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;

});


builder.Services.AddSwaggerGen( options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
        new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title="Magic Villa",
        Description="Brendans API to manage villa",
        TermsOfService = new Uri("https://www.google.com"),
        Contact = new OpenApiContact
        {
            Name = "Best Project",
            Url = new Uri("https://www.bing.com")
        },
        License = new OpenApiLicense
        {
            Name = "Best Project License",
            Url = new Uri("https://www.microsoft.com")
        }

    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Magic Villa",
        Description = "Brendans API to manage villa",
        TermsOfService = new Uri("https://www.google.com"),
        Contact = new OpenApiContact
        {
            Name = "Best Project",
            Url = new Uri("https://www.bing.com")
        },
        License = new OpenApiLicense
        {
            Name = "Best Project License",
            Url = new Uri("https://www.microsoft.com")
        }

    });



}    
    );
// to add the custom logger
builder.Services.AddSingleton<ILogging, LoggingV2>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(
    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
    ); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json","Magic_VillaV2");

    });

    //https://localhost:7001/swagger/v1/swagger.json


}

//////////////////////
///




////////////////////////

app.UseHttpsRedirection();

app.UseAuthentication();  // add before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
