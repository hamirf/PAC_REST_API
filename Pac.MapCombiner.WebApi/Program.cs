using System.Reflection;
using PAC_Map_Combiner_REST_API.Repositories;
using PAC_Map_Combiner_REST_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

rootPath = rootPath != null ? rootPath.Replace(rootPath.Substring(rootPath.LastIndexOf(@"bin\")), string.Empty) : throw new NullReferenceException();

var mapRepository = new MapRepository(Path.Combine(rootPath, "GroundEncoder"));
builder.Services.AddSingleton<IMapRepository>( mapRepository );
builder.Services.AddSingleton<IMapService, MapService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();