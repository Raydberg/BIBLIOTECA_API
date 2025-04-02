using System.Text;
using BIBLIOTECA_API.DB;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
var KeyJwt = Environment.GetEnvironmentVariable("key_jwt");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Configurar Identity Login and Logout
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// UserManager -> Servicio para admnistrar usuarios y se tiene que registrar y le pasamos la clase que representa un usuario
builder.Services.AddScoped<UserManager<IdentityUser>>();
// Servicio para autenticar usuarios
builder.Services.AddScoped<SignInManager<IdentityUser>>();
// Permitiar a los accesos HTTP desde cualquier clase
builder.Services.AddHttpContextAccessor();
// Agregar el tipo de autenticaion 
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    // asp.net core no cambie los claims de manera automatica 
    options.MapInboundClaims = false;
    // Validaciones para los tokens
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Metodos para la valdiacion del token 
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        // Validacion de la llave secreta
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeyJwt!)),
        ClockSkew = TimeSpan.Zero
    };
});


var app = builder.Build();


// Area de Middlewares


app.MapControllers();
app.Run();