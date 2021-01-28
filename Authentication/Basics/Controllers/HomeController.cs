using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob K. Foo"), // Correo del usuario
                new Claim("DrivingLicense", "A+"), // Dato personalizado del usuario
            };

            var grandmaClaims = new List<Claim>() // Datos del usuario
            {
                new Claim(ClaimTypes.Name, "Bob"), // Nombre del usuario
                new Claim(ClaimTypes.Email, "Bob@gmail.com"), // Correo del usuario
                new Claim("Grandma.Says", "Very Nice boi"), // Dato personalizado del usuario
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity"); // Identidad del usuario

            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Goverment"); // Identidad del usuario

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
