using BIBLIOTECA_API.DB;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
// Area de servicios 

// Configurar automapper en nuestra aplicacio
builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddTransient<>()

// Habilitar los controladores
//Configuracion de 
builder.Services.AddControllers().AddNewtonsoftJson();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
var app = builder.Build();


// Area de Middlewares


app.MapControllers();
app.Run();
