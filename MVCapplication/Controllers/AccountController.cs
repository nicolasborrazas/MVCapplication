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
                // Lógica de autenticación (aquí puedes configurar sesiones o cookies)
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }
}
