using Microsoft.EntityFrameworkCore;
using MVCapplication.Data; // Cambia "MVCapplication" por el nombre correcto de tu proyecto

var builder = WebApplication.CreateBuilder(args);

// Configuración de Entity Framework con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilitar el registro de errores en la consola
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Registrar servicios de autenticación, autorización y controladores
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();  // Este es el servicio faltante para usar controladores y vistas

var app = builder.Build();

// Configuración de manejo de excepciones
if (app.Environment.IsDevelopment())
{
    // Página detallada de excepciones para el entorno de desarrollo
    app.UseDeveloperExceptionPage();
}
else
{
    // Página personalizada de manejo de errores para producción
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirigir automáticamente a HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticación y autorización en el pipeline
app.UseAuthentication();  // Agregar el middleware de autenticación
app.UseAuthorization();   // El middleware de autorización

// Definir rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
