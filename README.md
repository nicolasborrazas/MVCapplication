### **README.md**: 

```md
# MVCapplication - Aplicación Básica en ASP.NET Core MVC

## Descripción
Este proyecto es una aplicación muy sencilla desarrollada en ASP.NET Core MVC con el propósito de entender y aprender la estructura básica de una aplicación MVC. La aplicación incluye un sistema de login básico, estructura HTML y CSS separada, y un layout de página utilizando HTML5.

## Tecnologías Utilizadas
- **ASP.NET Core MVC** para la estructura de la aplicación.
- **Entity Framework Core** para la interacción con la base de datos.
- **SQL Server** como base de datos local.
- **HTML5 y CSS** para la presentación.
- **C#** como lenguaje de backend.

## Características
- Estructura básica de una página HTML5 que incluye `header`, `nav`, `article`, `section`, `aside`, y `footer`.
- Sistema de autenticación básica con un formulario de login.
- Separación del HTML y CSS para mantener el código limpio.
- Uso de Entity Framework Core para la base de datos con migraciones.

## Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/yourusername/MVCapplication.git
```

### 2. Abrir el proyecto en Visual Studio
Abre **Visual Studio 2022** y selecciona la solución **MVCapplication.sln**.

### 3. Configuración de la base de datos
El proyecto usa **SQL Server LocalDB**. No es necesario instalar una base de datos externa.

Asegúrate de que la cadena de conexión esté correctamente configurada en el archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LoginAppDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Ejecutar las migraciones de Entity Framework
En **Visual Studio**, abre la **Package Manager Console** y ejecuta los siguientes comandos para crear la base de datos y aplicar las migraciones:

```bash
Add-Migration InitialCreate
Update-Database
```

Esto creará una tabla `Users` en la base de datos.

### 5. Ejecutar la aplicación
Presiona **F5** o haz clic en **Iniciar** para ejecutar la aplicación en tu navegador local.

## Estructura del Proyecto

### 1. **Program.cs**
Este archivo contiene la configuración principal de la aplicación y los servicios que se registran, como los controladores y Entity Framework Core.

```csharp
using Microsoft.EntityFrameworkCore;
using MVCapplication.Data; // Asegúrate de usar el nombre correcto del proyecto

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
builder.Services.AddControllersWithViews();  // Registrar controladores y vistas

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
app.UseAuthentication();  // Middleware de autenticación
app.UseAuthorization();   // Middleware de autorización

// Definir rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 2. **AppDbContext.cs**
Este archivo contiene la configuración del contexto de la base de datos usando Entity Framework Core.

```csharp
using Microsoft.EntityFrameworkCore;
using MVCapplication.Models;

namespace MVCapplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
```

### 3. **User.cs**
Este archivo define el modelo de usuario que utilizamos para el login.

```csharp
namespace MVCapplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
```

### 4. **AccountController.cs**
Este archivo gestiona el sistema de login y contiene las acciones de mostrar el formulario de login y manejar las solicitudes de autenticación.

```csharp
using Microsoft.AspNetCore.Mvc;
using MVCapplication.Data;
using MVCapplication.Models;
using System.Linq;

namespace MVCapplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la vista de login
        public IActionResult Login()
        {
            return View();
        }

        // Acción para procesar el formulario de login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            
            if (user != null)
            {
                // Lógica de autenticación (configurar sesiones o cookies)
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }
}
```

### 5. **Login.cshtml**
Esta es la vista del formulario de login.

```html
@{
    ViewData["Title"] = "Login";
}

<h2>Login</h2>

<form asp-action="Login" method="post">
    <div>
        <label for="username">Username</label>
        <input type="text" id="username" name="username" />
    </div>
    <div>
        <label for="password">Password</label>
        <input type="password" id="password" name="password" />
    </div>
    <button type="submit">Login</button>
</form>

@if (ViewBag.Error != null)
{
    <p style="color: red">@ViewBag.Error</p>
}
```

### 6. **Index.cshtml**
La estructura de la página principal (Home) de la aplicación.

```html
@{
    ViewData["Title"] = "Home";
}

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewData["Title"] - MVCapplication</title>
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <header>
        <h1>Bienvenido a MVCapplication</h1>
        <nav>
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/Account/Login">Login</a></li>
            </ul>
        </nav>
    </header>

    <article>
        <section>
            <h2>Sección 1</h2>
            <p>Contenido de la primera sección de la página.</p>
        </section>
        <section>
            <h2>Sección 2</h2>
            <p>Contenido de la segunda sección de la página.</p>
        </section>
    </article>

    <aside>
        <h3>Aside</h3>
        <p>Información adicional o enlaces en la barra lateral.</p>
    </aside>

    <footer>
        <p>Pie de página de la aplicación - MVCapplication</p>
    </footer>
</body>
</html>
```

### 7. **site.css**
Archivo CSS ubicado en `wwwroot/css/site.css`.

```css
body {
    font-family: Arial, sans-serif;
}

header, nav, article, section, aside, footer {
    border: 1px solid #ddd;
    padding: 10px;
    margin: 10px;
}

header, footer {
    background-color: #f2f2f2;
}

nav {
    background-color: #ddd;
}

nav ul {
    list-style-type: none;
    padding: 0;
}

nav ul li {
    display: inline;
    margin-right: 15px;
}

aside {
    float: right;
    width: 25%;
}

article {
    float: left;
    width: 70%;
}

footer {
    clear: both;
    text-align: center;
}
```

## Contribuciones
Si deseas contribuir a este proyecto, por favor abre un issue o envía un pull request. ¡Toda contribución es bienvenida!

## Licencia
Este proyecto está bajo la licencia MIT. Consulta el archivo [LICENSE](LICENSE) para más detalles.
```

### Explicación:
- El **README.md** contiene instrucciones detalladas sobre cómo instalar, configurar, y ejecutar el proyecto, además de la explicación completa de los archivos que forman

