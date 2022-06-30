using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.OpenApi.Models;
using Messanger.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Messenger.BusinessLogic.Pipelines;
using Messenger.ApplicationServices.Interfaces;
using Messenger.ApplicationServices.Services;
using Messenger.BusinessLogic.Commands.Users.Register;
using Messenger.Data;
using Messenger.Domain.Constants;
using FluentValidation;
using Messenger.BusinessLogic.Commands.Authentication.Login;
using Messenger.BusinessLogic.Hubs;
using Messenger.Presentation.Middlewares;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .Build();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSecret = Encoding.ASCII.GetBytes(configuration["JWTAuth:Secret"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),

            ValidateAudience = false,
            ValidateIssuer = false,

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false;

        var tokenHandler = options.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().Single();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
    });
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddSwaggerGen(options =>
{
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Put Your access token here (drop **Bearer** prefix):",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.OperationFilter<OpenApiAuthFilter>();
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(DatabaseConstants.ConnectionString);
});

builder.Services.AddScoped<IJwtGenerator,JwtGenerator>();

builder.Services.AddMediatR(typeof(RegisterUserCommand).Assembly);

builder.Services.AddHttpContextAccessor();

//pipeline
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdPipe<,>));

//validation
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());



var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotifyHub>("/notify");
});
    


app.Run();
