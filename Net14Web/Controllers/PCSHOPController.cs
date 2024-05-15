using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net14Web.DbStuff.Repositories.PcShop;
using Net14Web.Models.PcShop;
using Net14Web.Models.PCSHOP;
using Net14Web.Services;
using Net14Web.DbStuff.Models;
using Net14Web.DbStuff.Repositories;
using Microsoft.AspNetCore.SignalR;
using Net14Web.Hubs;
using Net14Web.Services.ApiServices;
using Net14Web.Controllers.CustomAuthAttributes;
using Net14Web.DbStuff.Models.Movies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Net14Web.DbStuff.Repositories.Movies;


namespace Net14Web.Controllers
{
    public class PcShopController : Controller
    {
        private UserRepository _userRepository;
        private PcsRepositoryPcShop _pcsRepositoryPcShop;
        private AuthService _authService;
        private AlertRepository _alertRepository;
        private IHubContext<AlertHub, IAlertHub> _alertHub;
        private DateApi _dateApi;
        private FoxApi _foxApi;
        private RoleRepository _roleRepository;
        private CpuRepositoryPcShop _cpuRepositoryPcShop;
        private IHttpContextAccessor _httpContextAccessor;
        public const string AUTH_KEY = "Smile";


        public PcShopController(UserRepository userRepositoryPcShop, PcsRepositoryPcShop pcsRepositoryPcShop,
            AuthService authService, AlertRepository alertRepository,
            IHubContext<AlertHub, IAlertHub> alertHub,
            DateApi dateApi,
            FoxApi foxApi,
            RoleRepository roleRepository,
            CpuRepositoryPcShop cpuRepositoryPcShop,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepositoryPcShop;
            _pcsRepositoryPcShop = pcsRepositoryPcShop;
            _authService = authService;
            _alertRepository = alertRepository;
            _alertHub = alertHub;
            _dateApi = dateApi;
            _foxApi = foxApi;
            _roleRepository = roleRepository;
            _cpuRepositoryPcShop = cpuRepositoryPcShop;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModelPcShop();
            viewModel.UserName = _authService.GetCurrentUserName();
            var currentUser = _authService.GetCurrentUser();
            viewModel.IsGuestUser = currentUser is null;

            viewModel.CanDelete = CurrentUserCanDelete();

            var date = DateTime.Now;
            var dateFact = _dateApi.GetFactAboutDate(date.Month, date.Day);
            var foxDtoTask = _foxApi.GetRandomFoxUrl();

            Task.WaitAll(dateFact, foxDtoTask);

            viewModel.FactAboutDate = dateFact.Result;
            viewModel.FoxUrl = foxDtoTask.Result?.image ?? "";
            return View(viewModel);
        }
        [AdminOnly]
        public ActionResult Users()
        {
            var dbUserPcShop = _userRepository.GetUsers(10);

            var viewModels = dbUserPcShop
                .Select(dbUserPcShop => new UserViewModel
                {
                    Id = dbUserPcShop.Id,
                    FirstName = dbUserPcShop.FirstName,
                    Login = dbUserPcShop.Login,
                    Email = dbUserPcShop.Email,
                    Password = dbUserPcShop.Password,
                })
                .ToList();

            return View(viewModels);
        }

        public ActionResult Cpu()
        {
            var dbCpuModel = _cpuRepositoryPcShop.GetCpu(10);
            return View(dbCpuModel);
        }

        [Authorize]
        public ActionResult PCs()
        {
            var dbPCModel = _pcsRepositoryPcShop.GetPCs(10);
            return View(dbPCModel);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(AddUserViewModel UserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(UserViewModel);
            }
            var user = new User
            {
                FirstName = UserViewModel.FirstName,
                Login = UserViewModel.Login,
                Email = UserViewModel.Email,
                Password = UserViewModel.Password,
            };
            _userRepository.Registration(user);

            var alert = new Alert()
            {
                Creater = _authService.GetCurrentUser() ?? _userRepository.GetUsers(1).First(),
                Message = $"{user.FirstName} присоеденился к нам",
            };
            _alertRepository.Add(alert);

            await _alertHub.Clients.All.PushAlert(alert.Message, alert.Id);
            return RedirectToAction(nameof(Index));

        }
        [AdminOnly]
        public ActionResult EditUserPassword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult EditUserPassword(int id, string password)
        {
            _userRepository.EditUserPassword(id, password);
            return RedirectToAction(nameof(Index));
        }

        // GET: PCSHOPController/Delete/5

        public ActionResult DeleteUsers()
        {
            if (CurrentUserCanDelete())
            {
                return View();
            }
            else { return StatusCode(403); }
        }

        // POST: PCSHOPController/Delete/5
        [HttpGet]

        public ActionResult DeleteUsers(int id)
        {
            if (CurrentUserCanDelete())
            {
                _userRepository.DeleteUsers(id);
                return RedirectToAction(nameof(Users));
            }
            else { return StatusCode(403); }

        }

        private bool CurrentUserCanDelete()
        {
            var currentUser = _authService.GetCurrentUser();
            var roles = currentUser is not null ? _roleRepository.GetUserRolesAndPermissions(currentUser) : null;
            var canDelete = false;
            if (roles is not null)
            {
                canDelete = roles.Any(r => r.Permissions is not null && r.Permissions.Any(p => p.Type == PermissionType.DeleteUser));
            }
            return canDelete;
        }
        public async Task<IActionResult> SwitchLocale(string locale)
        {
            var userId = _authService.GetCurrentUserId().Value;
            _userRepository.SwitchLocal(userId, locale);

            var oldClaims = _httpContextAccessor?.HttpContext?.User.Claims!;

            var claims = new List<Claim>
            {
                new Claim("id", oldClaims.First(x => x.Type == "id").Value),
                new Claim("name", oldClaims.First(x => x.Type == "name").Value),
                new Claim("email", oldClaims.First(x => x.Type == "email").Value),
                new Claim(AuthService.LOCALE_TYPE, locale),
                new Claim("avatar", oldClaims.First(x => x.Type == "avatar").Value),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims", oldClaims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims").Value)
            };

            var identity = new ClaimsIdentity(claims, AUTH_KEY);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AUTH_KEY, principal);

            return RedirectToAction("Index");
        }
    }
}