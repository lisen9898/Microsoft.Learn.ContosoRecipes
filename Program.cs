using ContosoRecipesApi.Models;
using ContosoRecipesApi.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ContosoRecipesDatabaseSettings>(builder.Configuration.GetSection("ContosoRecipesDatabase"));
builder.Services.AddSingleton<RecipesService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Contoso Recipe API",
            Description = "A sample ASP.NET Web API that allows you work with recipe data.",
            Contact = new OpenApiContact()
            {
                Name = "Gavin Lee",
                Email = "lisen9898@hotmail.com",
                Url = new Uri("https://twitter.com/wankphilia"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            },
            Version = "v1"
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        c.IncludeXmlComments(xmlPath);

        c.CustomOperationIds(apiDescription =>
        {
            return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
        });
    }).AddSwaggerGenNewtonsoftSupport();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "ContosoRecipes V1");
        o.DisplayOperationId();
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
