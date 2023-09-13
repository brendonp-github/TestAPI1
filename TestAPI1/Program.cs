using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using TestAPI1.Data;
using TestAPI1.Data.Models;

var corsPolicy = "CorsPolicy";
if (args == null)
{
    args = new string[] { corsPolicy };
}
else
{
    var list = new List<string>(args);
    list.Add(corsPolicy);
    args = list.ToArray();
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDBContext>()
    //.AddDefaultTokenProviders()
    ;

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["jwtSettings:Issuer"],
        ValidAudience = builder.Configuration["jwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:SecurityKey"]))
    };
});


builder.Services.AddScoped<JwtHandler>();
builder.Services.AddApplicationInsightsTelemetry();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy",
//        builder => builder
//            .AllowAnyMethod()
//            .AllowCredentials()
//            .SetIsOriginAllowed((host) => true)
//            .AllowAnyHeader());
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: corsPolicy,
//                      policy =>
//                      {
//                          policy.WithOrigins(
//                                "https://localhost:4200/",
//                                "http://localhost:4200/",
//                                "https://localhost:5070/",
//                                "http://localhost:5070/",
//                                "https://localhost:8001/",
//                                "http://localhost:8001/"
//                                );
//                      });
//});

var app = builder.Build();

//app.UseCors(builder => builder
//       .AllowAnyHeader()
//       .AllowAnyMethod()
//       .AllowAnyOrigin()
//    );

//app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
    //app.UseCors("CorsPolicy");
    var validOrigins = builder.Configuration.GetSection("ValidOrigins").Get<string[]>();
    app.UseCors(builder => builder.WithOrigins(validOrigins!.ToArray())
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
