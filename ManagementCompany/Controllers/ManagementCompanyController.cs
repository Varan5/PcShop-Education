﻿using ManagementCompany.BusinessServices;
using ManagementCompany.Controllers.CustomAuthAttributes;
using ManagementCompany.DbStuff.Models;
using ManagementCompany.DbStuff.Models.Enums;
using ManagementCompany.DbStuff.Repositories;
using ManagementCompany.Models;
using ManagementCompany.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagementCompany.Controllers
{
    public class ManagementCompanyController : Controller
    {
        private CompanyRepository _companyRepository;
        private ProjectRepository _projectRepository;
        private UserRepository _userRepository;
        private ArticleRepository _articleRepository;
        private UserTaskRepository _userTaskRepository;
        private MemberPermissionRepository _memberPermissionRepository;
        private MemberStatusRepository _memberStatusRepository;
        private TaskStatusRepository _taskStatusRepository;
        private AuthService _authService;
        private UserBusinessService _userBusinessService;

        private IWebHostEnvironment _webHostEnvironment;

        public ManagementCompanyController(
            CompanyRepository companyRepository,
            ProjectRepository projectRepository,
            UserRepository userRepository,
            UserTaskRepository userTaskRepository,
            MemberPermissionRepository memberPermissionRepository,
            MemberStatusRepository memberStatusRepository,
            AuthService authService,
            IWebHostEnvironment webHostEnvironment,
            TaskStatusRepository taskStatusRepository,
            ArticleRepository articleRepository,
            UserBusinessService userBusinessService)
        {
            _companyRepository = companyRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _userTaskRepository = userTaskRepository;
            _memberPermissionRepository = memberPermissionRepository;
            _memberStatusRepository = memberStatusRepository;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _taskStatusRepository = taskStatusRepository;
            _articleRepository = articleRepository;
            _userBusinessService = userBusinessService;
        }

        public IActionResult Index()
        {
            var user = _authService.GetCurrentUser();

            var dbAllTasks = _userTaskRepository.GetAll()
                .ToList();

            var dbWorkTasks = _userTaskRepository.GetInPgogressTasks();

            var dbCompletedTasks = _userTaskRepository.GetCompletedTasks();

            var dbProjects = _projectRepository
                .GetAll();

            var viewModel = new IndexViewModel();

            CheckUser(viewModel, user);

            viewModel.AllTasks = dbAllTasks
                .Select(dbAllTask => new TaskViewModel
                {
                    Name = dbAllTask.Name
                })
                .ToList();

            viewModel.WorkInProgressTasks = dbWorkTasks
                .Select(dbWorkTask => new TaskViewModel
                {
                    Name = dbWorkTask.Name
                })
                .ToList();

            viewModel.CompletedTasks = dbCompletedTasks
                .Select(dbCompletedTask => new TaskViewModel
                {
                    Name = dbCompletedTask.Name
                })
                .ToList();

            viewModel.Projects = dbProjects
                .Select(dbProject => new ProjectViewModel
                {
                    ProjectName = dbProject.Name
                })
                .ToList();

            viewModel.Users = _userBusinessService.GetUsers();

            return View(viewModel);
        }

        public IActionResult IndexUsers()
        {
            var dbUsers = _userRepository.GetUsers();

            var dbAllTasks = _userTaskRepository.GetAll();

            var dbWorkTasks = _userTaskRepository.GetInPgogressTasks();

            var dbCompletedTasks = _userTaskRepository.GetCompletedTasks();

            var dbProjects = _projectRepository.GetAll();

            var viewModel = new IndexViewModel();

            viewModel.AllTasks = dbAllTasks
                .Select(dbAllTask => new TaskViewModel
                {
                    Name = dbAllTask.Name
                })
                .ToList();

            viewModel.WorkInProgressTasks = dbWorkTasks
                .Select(dbWorkTask => new TaskViewModel
                {
                    Name = dbWorkTask.Name
                })
                .ToList();

            viewModel.CompletedTasks = dbCompletedTasks
                .Select(dbCompletedTask => new TaskViewModel
                {
                    Name = dbCompletedTask.Name
                })
                .ToList();

            viewModel.Projects = dbProjects
                .Select(dbProject => new ProjectViewModel
                {
                    ProjectName = dbProject.Name
                })
                .ToList();

            viewModel.Users =
                dbUsers
                .Select(dbUser => new UserViewModel
                {
                    Id = dbUser.Id,
                    NickName = dbUser.NickName,
                    MemberPermission = dbUser.MemberPermission?.Permission,
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult IndexAdmins()
        {
            var dbUsers = _userRepository.GetManagers();

            var dbAllTasks = _userTaskRepository.GetAll();

            var dbWorkTasks = _userTaskRepository.GetInPgogressTasks();

            var dbCompletedTasks = _userTaskRepository.GetCompletedTasks();

            var dbProjects = _projectRepository.GetAll();

            var viewModel = new IndexViewModel();

            viewModel.AllTasks = dbAllTasks
                .Select(dbAllTask => new TaskViewModel
                {
                    Name = dbAllTask.Name
                })
                .ToList();

            viewModel.WorkInProgressTasks = dbWorkTasks
                .Select(dbWorkTask => new TaskViewModel
                {
                    Name = dbWorkTask.Name
                })
                .ToList();

            viewModel.CompletedTasks = dbCompletedTasks
                .Select(dbCompletedTask => new TaskViewModel
                {
                    Name = dbCompletedTask.Name
                })
                .ToList();

            viewModel.Projects = dbProjects
                .Select(dbProject => new ProjectViewModel
                {
                    ProjectName = dbProject.Name
                })
                .ToList();

            viewModel.Users =
                dbUsers
                .Select(dbUser => new UserViewModel
                {
                    Id = dbUser.Id,
                    NickName = dbUser.NickName,
                    MemberPermission = dbUser.MemberPermission?.Permission,
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult Blog()
        {
            var dbArticles = _articleRepository.GetAll();

            var blogViewModel = new BlogViewModel();
            blogViewModel.UserNickName = _authService.GetCurrentUserNickName();

            blogViewModel.Articles =
                dbArticles
                .Select(dbArticle => new ArticleViewModel
                {
                    Id = dbArticle.Id,
                    Title = dbArticle.Title,
                    Description = dbArticle.Description,
                    ThumbsUp = dbArticle.ThumbsUp,
                    ThumbsDown = dbArticle.ThumbsDown,
                })
                .ToList();

            blogViewModel.Articles.Count();

            return View(blogViewModel);
        }

        public IActionResult About()
        {
            var model = new BaseViewModel();
            return View(model);
        }

        public IActionResult Contacts()
        {
            var model = new BaseViewModel();
            return View(model);
        }

        public IActionResult LogError()
        {
            var model = new BaseViewModel();
            return View(model);
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AdminPanel()
        {
            var dbStatuses = _memberStatusRepository.GetAll();

            var dbPermissions = _memberPermissionRepository.GetAll();

            var dbCompanies = _companyRepository.GetCompaniesWithStatus();

            var dbProjects = _projectRepository.GetProjectsWithStatus();

            var dbExecuters = _userRepository.GetExecutors();

            var viewModel = new AdminPanelViewModel();

            viewModel.Statuses = dbStatuses
                .Select(dbStatus => new StatusViewModel
                {
                    Id = dbStatus.Id,
                    Status = dbStatus.Status,
                })
                .ToList();

            viewModel.Permissions = dbPermissions
                .Select(dbPermission => new PermissionViewModel
                {
                    Id = dbPermission.Id,
                    Permission = dbPermission.Permission,
                })
                .ToList();

            viewModel.Companies = dbCompanies
                .Select(dbCompany => new CompanyViewModel
                {
                    Id = dbCompany.Id,
                    CompanyName = dbCompany.Name,
                    CompanyShortName = dbCompany.ShortName,
                    CompanyAdress = dbCompany.Adress,
                    CompanyEmail = dbCompany.Email,
                    CompanyPhoneNumber = dbCompany.PhoneNumber,
                    CompanyStatus = dbCompany.Status.Status
                })
                .ToList();

            viewModel.Projects = dbProjects
                .Select(dbProject => new ProjectViewModel
                {
                    Id = dbProject.Id,
                    ProjectName = dbProject.Name,
                    ProjectShortName = dbProject.ShortName,
                    ProjectAdress = dbProject.Adress,
                    ProjectStatus = dbProject.Status.Status,
                    ProjectLinkCompany = dbProject.Company.Name
                })
                .ToList();

            viewModel.Executors = dbExecuters
                .Select(dbExecuter => new ExecutorViewModel
                {
                    Id = dbExecuter.Id,
                    FirstName = dbExecuter.FirstName,
                    LastName = dbExecuter.LastName,
                    NickName = dbExecuter.NickName,
                    Email = dbExecuter.Email,
                    PhoneNumber = dbExecuter.PhoneNumber,
                    ExpireDate = dbExecuter.ExpireDate,
                    //Company = dbExecuter.Company.Name,
                    MemberPermission = _memberPermissionRepository.GetById(dbExecuter.MemberPermission.Id).Permission,
                    MemberStatus = _memberStatusRepository.GetById(dbExecuter.Status.Id).Status
                })
                .ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _authService.GetCurrentMcUser();

            var userTasks = _userTaskRepository.GetCurrentUserTasks(user);

            var model = new ProfileViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
            };
            model.CurrentUserTasks = userTasks.Select(userTask => new TaskViewModel
            {
                Name = userTask.Name,
                Description = userTask.Description,
                Status = userTask.Status?.Status ?? "---",
                CreationDate = userTask.CreationDate,
                StartDate = userTask.StartDate,
                CompletionDate = userTask.CompletionDate
            })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel model, int id)
        {
            _userRepository.UpdateUser(model, id);

            var user = _authService.GetCurrentMcUser();

            var userTasks = _userTaskRepository.GetCurrentUserTasks(user);

            model.CurrentUserTasks = userTasks.Select(userTask => new TaskViewModel
            {
                Name = userTask.Name,
                Description = userTask.Description,
                Status = userTask.Status?.Status ?? "---",
                CreationDate = userTask.CreationDate,
                StartDate = userTask.StartDate,
                CompletionDate = userTask.CompletionDate
            })
                .ToList();

            return View(model);
        }

        public IActionResult Registration()
        {
            var model = new RegistrationViewModel();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel registrationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registrationViewModel);
            }

            var user = new User
            {
                Email = registrationViewModel.Email,
                NickName = registrationViewModel.NickName,
                Password = registrationViewModel.Password,
                MemberPermission = _memberPermissionRepository.GetById((int)MemberPermissionEnum.User),
                Status = _memberStatusRepository.GetById((int)MemberStatusEnum.Active)
            };

            try
            {
                _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("RegError");
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AddPermission()
        {
            return View();
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult AddPermission(PermissionViewModel viewModel)
        {
            var permission = new MemberPermission
            {
                Permission = viewModel.Permission
            };

            _memberPermissionRepository.Add(permission);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult UpdatePermission(int permissionId)
        {
            if (permissionId > 0)
            {
                var permission = _memberPermissionRepository.GetById(permissionId);

                var viewModel = new PermissionViewModel()
                {
                    Id = permissionId,
                    Permission = permission.Permission
                };

                return View(viewModel);
            }

            return NotFound("Not Found");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult UpdatePermission(PermissionViewModel viewModel, int id)
        {
            _memberPermissionRepository.UpdatePermission(viewModel, id);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult RemovePermission(int id)
        {
            _memberPermissionRepository.Delete(id);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AddStatus()
        {
            return View();
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult AddStatus(StatusViewModel viewModel)
        {
            var status = new MemberStatus
            {
                Status = viewModel.Status
            };

            _memberStatusRepository.Add(status);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult UpdateStatus(int statusId)
        {
            if (statusId > 0)
            {
                var status = _memberStatusRepository.GetById(statusId);

                var viewModel = new StatusViewModel()
                {
                    Id = statusId,
                    Status = status.Status
                };

                return View(viewModel);
            }

            return NotFound("Not Found");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult UpdateStatus(StatusViewModel viewModel, int id)
        {
            _memberStatusRepository.UpdateStatus(viewModel, id);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult RemoveStatus(int id)
        {
            _memberStatusRepository.Delete(id);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult AddCompany(CompanyViewModel companyViewModel)
        {
            companyViewModel.Companies = _companyRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            companyViewModel.Projects = _projectRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            companyViewModel.Permissions = _memberPermissionRepository.GetAll()
                .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                .ToList();

            companyViewModel.Statuses = _memberStatusRepository.GetAll()
                .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                .ToList();

            var company = new Company
            {
                Name = companyViewModel.CompanyName,
                ShortName = companyViewModel.CompanyShortName,
                Email = companyViewModel.CompanyEmail,
                Adress = companyViewModel.CompanyAdress,
                PhoneNumber = companyViewModel.CompanyPhoneNumber,
                Status = _memberStatusRepository.GetById((int)MemberStatusEnum.Active)
            };

            _companyRepository.Add(company);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult UpdateCompany(int companyId)
        {
            if (companyId > 0)
            {
                var company = _companyRepository.GetCompaniesWithStatus()
                .SingleOrDefault(x => x.Id == companyId);

                var viewModel = new CompanyViewModel();

                viewModel.Id = company.Id;
                viewModel.CompanyName = company.Name;
                viewModel.CompanyShortName = company.ShortName;
                viewModel.CompanyAdress = company.Adress;
                viewModel.CompanyEmail = company.Email;
                viewModel.CompanyPhoneNumber = company.PhoneNumber;
                viewModel.CompanyStatus = company.Status.Status;
                viewModel.Statuses = _memberStatusRepository.GetAll()
                    .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                    .ToList();

                return View(viewModel);
            }

            return NotFound("Not Found");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult UpdateCompany(CompanyViewModel companyViewModels, int id, int statusId)
        {
            _companyRepository.UpdateCompany(companyViewModels, id, statusId);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult RemoveCompany(int id)
        {
            _companyRepository.Delete(id);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AddProject()
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();

            projectViewModel.Companies = _companyRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            projectViewModel.Projects = _projectRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            projectViewModel.Permissions = _memberPermissionRepository.GetAll()
                .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                .ToList();

            projectViewModel.Statuses = _memberStatusRepository.GetAll()
                .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                .ToList();

            return View(projectViewModel);
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult AddProject(ProjectViewModel projectViewModel, int companyId)
        {
            projectViewModel.Companies = _companyRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            projectViewModel.Projects = _projectRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            projectViewModel.Permissions = _memberPermissionRepository.GetAll()
                .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                .ToList();

            projectViewModel.Statuses = _memberStatusRepository.GetAll()
                .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                .ToList();

            var project = new Project
            {
                Name = projectViewModel.ProjectName,
                ShortName = projectViewModel.ProjectShortName,
                Adress = projectViewModel.ProjectAdress,
                Status = _memberStatusRepository.GetById((int)MemberStatusEnum.Active),
                Company = _companyRepository.GetById(companyId)
            };

            _projectRepository.Add(project);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult RemoveProject(int id)
        {
            _projectRepository.Delete(id);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult AddExecutor()
        {
            var executorViewModel = new ExecutorViewModel();

            executorViewModel.Companies = _companyRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            executorViewModel.Projects = _projectRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            executorViewModel.Permissions = _memberPermissionRepository.GetAll()
                .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                .ToList();

            executorViewModel.Statuses = _memberStatusRepository.GetAll()
                .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                .ToList();

            return View(executorViewModel);
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult AddExecutor(ExecutorViewModel executorViewModel, int companyId, int projectId, int permissionId)
        {
            executorViewModel.Companies = _companyRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            executorViewModel.Projects = _projectRepository.GetAll()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            executorViewModel.Permissions = _memberPermissionRepository.GetAll()
                .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                .ToList();

            executorViewModel.Statuses = _memberStatusRepository.GetAll()
                .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                .ToList();

            _userBusinessService.AddExecutor(executorViewModel);

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        [AdminOnly]
        public IActionResult UpdateExecutor(int id)
        {
            var viewModel = new ExecutorViewModel();

            if (id != 0)
            {
                var user = _userRepository.GetExecutor(id);

                viewModel.Id = user.Id;
                viewModel.FirstName = user.FirstName;
                viewModel.LastName = user.LastName;
                viewModel.Email = user.Email;
                viewModel.PhoneNumber = user.PhoneNumber;
                viewModel.ExpireDate = user.ExpireDate;
                viewModel.Password = user.Password;
                viewModel.MemberPermission = user.MemberPermission.Permission;
                viewModel.MemberStatus = user.Status.Status;
                viewModel.NickName = user.NickName;
                viewModel.Company = user.Company.Name;
                viewModel.Companies = _companyRepository.GetAll()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                    .ToList();
                viewModel.Projects = _projectRepository.GetAll()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                    .ToList();
                viewModel.Statuses = _memberStatusRepository.GetAll()
                    .Select(x => new SelectListItem(x.Status, x.Id.ToString()))
                    .ToList();
                viewModel.Permissions = _memberPermissionRepository.GetAll()
                    .Select(x => new SelectListItem(x.Permission, x.Id.ToString()))
                    .ToList();

            }
            return View(viewModel);
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult UpdateExecutor(ExecutorViewModel executorViewModels, int id, int statusId, int companyId, int projectId, int permissionId)
        {
            _userRepository.UpdateExecutor(executorViewModels, id, statusId, companyId, projectId, permissionId);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [AdminOnly]
        public IActionResult RemoveExecutor(int id)
        {
            _userRepository.Delete(id);

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        public IActionResult RemoveUser(int id)
        {
            _userRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateUser(int id, string nickName)
        {
            _userRepository.UpdateUser(id, nickName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddTask() 
        {
            var model = new TaskViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddTask(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var a = _taskStatusRepository.GetById((int)UserTaskStatusEnum.Create);

            var task = new UserTask
            {
                Name = model.Name,
                Description = model.Description,
                Status = _taskStatusRepository.GetById((int)UserTaskStatusEnum.Create),
                Author = _authService.GetCurrentMcUser()
            };

            _userTaskRepository.Add(task);

            return RedirectToAction("Profile", new {Id = task.Author.Id});
        }

        private void CheckUser(BaseViewModel model, User user)
        {
            if (user is not null)
            {
                if (user.MemberPermission.Id == 5)
                {
                    model.IsUser = true;
                }
                else
                {
                    model.IsAdmin = true;
                }
            }
            else
            {
                model.IsGuest = true;
            }
        }
    }
}
