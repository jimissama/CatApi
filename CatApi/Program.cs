using System.Reflection;
using System.Resources;
using CatApi.MapperProfiles;
using CatApi.Repositories;
using CatApi.Repositories.Interfaces;
using CatApi.Services;
using CatApi.Services.Database;
using CatApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSingleton(typeof(HttpClient));
builder.Services.AddScoped(typeof(ICatRepo), typeof(CatRepo));
builder.Services.AddScoped(typeof(ITagRepo), typeof(TagRepo));
builder.Services.AddScoped(typeof(ICatTagRepo), typeof(CatTagRepo));
builder.Services.AddScoped(typeof(ICatService), typeof(CatService));
builder.Services.AddScoped(typeof(ITagService), typeof(TagService));
builder.Services.AddScoped(typeof(ICatTagService), typeof(CatTagService));
//builder.Services.AddScoped(typeof(IStoreImageService), typeof(StoreImageService));

builder.Services.AddSingleton(typeof(IHttpRequestService), typeof(HttpRequestService));
builder.Services.AddSingleton(typeof(ICatControllerValidationService), typeof(CatControllerValidationService));
builder.Services.AddSingleton(new ResourceManager("CatApi.Resources.stringResources", typeof(CatControllerValidationService).Assembly));

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();


builder.Services.AddDbContext<CatDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddSwaggerGen(c =>
    {
        c.ExampleFilters();
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatApi", Version = "v1" });
    });

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddAutoMapper(typeof(MappingSourceCatProfile));
builder.Services.AddAutoMapper(typeof(MappingEntityCatProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapSwagger().RequireAuthorization();


app.Run();
