using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LabsysteDoubledContext>(options =>
{
    options.UseMySql("server=labsystec.net;database=labsyste_doubled;user=labsyste_doubled;password=xs~o714N5", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb"));
});
builder.Services.AddTransient<ActividadesRepository>();
builder.Services.AddTransient<DepartamentosRepository>();
builder.Services.AddTransient<ActividadValidator>();
builder.Services.AddTransient<DepartamentosValidator>();
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["JwtSettings:SecretKey"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", app =>
    {
        app.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104857600;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MyPolicy");
app.MapControllers();
app.Run();
