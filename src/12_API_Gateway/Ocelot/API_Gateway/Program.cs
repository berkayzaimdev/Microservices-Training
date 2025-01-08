using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddOcelot(builder.Configuration);

// builder.Services.AddAuthorization();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
//     {
//         opts.TokenValidationParameters = new()
//         {
//             ValidateAudience = true,
//             ValidateIssuer = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Token:Issuer"],
//             ValidAudience = builder.Configuration["Token:Audience"],
//             IssuerSigningKey =
//                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
//             ClockSkew = TimeSpan.Zero
//         };
//     });

var app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

await app.UseOcelot();
app.UseHttpsRedirection();

app.Run();