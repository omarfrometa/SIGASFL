using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using SIGASFL.Repositories;
using SIGASFL.Restful.Extensions;
using SIGASFL.Services.Mapper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiHelp();

builder.Services.AddCors(options => {
    options.AddPolicy("cors", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var configMap = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
builder.Services.AddSingleton(configMap.CreateMapper());


builder.Services.AddDbContext<ApplicationContext>(o =>
{
    o.UseSqlServer(config.GetConnectionString("ApplicationContext"));
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddCommonServices(config);
builder.Services.AddAuthenticationConfig(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiHelp(config);

app.UseHttpsRedirection();
app.UseCors(config => config
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());

app.UseStaticFiles();
/*app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files")),
    RequestPath = new PathString("/Files")
});*/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
