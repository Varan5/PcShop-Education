using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Net14Web.DbStuff.Repositories.Movies;
using Net14Web.Models.Auth;
using Net14Web.Services;
using System.Security.Claims;
using User = Net14Web.DbStuff.Models.Movies.User;

namespace Net14Web.Controllers
{
    public class AuthController : Controller
    {
        private UserRepository _userRepository;

        public const string AUTH_KEY = "Smile";

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("PCSHOP/login")]
        public IActionResult Login()
        {
            return View("~/Views/PCSHOP/Login.cshtml");
        }

        [HttpPost]
        [Route("PCSHOP/login")]
        public IActionResult Login(AuthViewModel authViewModel)
        {
            var user = _userRepository.GetUserByLoginAndPassword(authViewModel.UserName, authViewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(nameof(AuthViewModel.UserName), "Wrong name or passwrod");
                return View(authViewModel);
            }

            SignInUser(user);

            return RedirectToAction("Index", "/");
        }

        [Route("PCSHOP/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "PCSHOP");
        }

        private string GetInfoFromClaims(IEnumerable<Claim> claims, string claimType)
        {
            return claims.First(x => x.Type == claimType).Value;
        }

        private void SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Login ?? "user"),
                new Claim("email", user.Email ?? ""),
                new Claim(AuthService.LOCALE_TYPE, user.PreferLocale),
                new Claim("avatar", user.AvatarUrl ?? ""),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims", user.Email ?? ""),
            };

            var identity = new ClaimsIdentity(claims, AUTH_KEY);
            var principal = new ClaimsPrincipal(identity);
            HttpContext
                .SignInAsync(AUTH_KEY, principal)
                .Wait();

            //return RedirectToAction("Index", "Home");
        }
    }
}
