using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net14Web.DbStuff.Repositories.Movies;
using Net14Web.Models;
using Net14Web.Services;
using Net14Web.Services.ApiServices;
using System.Diagnostics;

namespace Net14Web.Controllers
{
    public class HomeController : Controller
    {
        private AuthService _authService;
        private UserRepository _userRepository;

        public HomeController(AuthService authService,
            UserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult SwitchLocale(string locale)
        {
            var userId = _authService.GetCurrentUserId().Value;
            _userRepository.SwitchLocal(userId, locale);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}