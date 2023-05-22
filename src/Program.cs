using DsPair.src.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

DsService.getInstance();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
  options.SwaggerDoc("v1", new OpenApiInfo {
    Version = "v1",
    Title = "DsPair",
    Description = "Conecte seu DualShock 4 com facilidade ao Windows.",
    Contact = new OpenApiContact {
      Name = "GitHub",
      Url = new Uri("https://github.com/reedbluue/DsPair")
    },
    License = new OpenApiLicense {
      Name = "Apache License",
      Url = new Uri("https://github.com/reedbluue/DsPair/blob/master/LICENSE")
    }
  });

  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseExceptionHandler("/error");

if(!app.Environment.IsProduction()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
