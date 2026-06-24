using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using VisitorMVC.Models.DTOs;
using VisitorMVC.Models.ViewModels;
using VisitorMVC.Services.Interfaces;

namespace VisitorMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController( IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            string token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var role = HttpContext.Session.GetString("UserRole");
                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (role == "Employee")
                {
                    return RedirectToAction("Index", "VisitorRequest");
                }
            }

            return View();
        }

        

        //[ValidateAntiForgeryToken]

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _authService.LoginAsync(dto);

            if (response == null)
            {
                ViewBag.Error ="Invalid username or password";

                return View(dto);
            }

            var loginResponse = JsonConvert.DeserializeObject <LoginResponseViewModel>(response);

            HttpContext.Session.SetString("JWToken",loginResponse.Token);

            HttpContext.Session.SetString("UserRole",loginResponse.Role);

            HttpContext.Session.SetString("Username",loginResponse.Username);

            if (loginResponse.Role == "Admin")
            {
                return RedirectToAction("Index","Admin");
            }

            return RedirectToAction("Index","VisitorRequest");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}