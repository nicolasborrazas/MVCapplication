using Microsoft.EntityFrameworkCore;
using MVCapplication.Data; // Cambia "MVCapplication" por el nombre correcto de tu proyecto

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Entity Framework con la cadena de conexi�n
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilitar el registro de errores en la consola
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Registrar servicios de autenticaci�n, autorizaci�n y controladores
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();  // Este es el servicio faltante para usar controladores y vistas

var app = builder.Build();

// Configuraci�n de manejo de excepciones
if (app.Environment.IsDevelopment())
{
    // P�gina detallada de excepciones para el entorno de desarrollo
    app.UseDeveloperExceptionPage();
}
else
{
    // P�gina personalizada de manejo de errores para producci�n
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirigir autom�ticamente a HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticaci�n y autorizaci�n en el pipeline
app.UseAuthentication();  // Agregar el middleware de autenticaci�n
app.UseAuthorization();   // El middleware de autorizaci�n

// Definir rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
