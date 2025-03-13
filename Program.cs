using BIBLIOTECA_API.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Area de servicios 

// Configurar automapper en nuestra aplicacio
builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddTransient<>()

// Habilitar los controladores
//Configuracion de 
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();


// Area de Middlewares


app.MapControllers();
app.Run();
