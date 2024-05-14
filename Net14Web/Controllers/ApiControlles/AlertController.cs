﻿using Microsoft.AspNetCore.Mvc;
using Net14Web.DbStuff.Repositories;
using Net14Web.Models.Alerts;
using Net14Web.Services;

namespace Net14Web.Controllers.ApiControlles
{
    [ApiController]
    [Route("/api/alert/{action}")]
    public class AlertController : Controller
    {
        private AlertRepository _alertRepository;
        private AuthService _authService;

        public AlertController(AlertRepository alertRepository, AuthService authService)
        {
            _alertRepository = alertRepository;
            _authService = authService;
        }

        public List<AlertShortInfoViewModel> GetExistedAlerts()
        {
            var userId = _authService.GetCurrentUserId();
            if (!userId.HasValue)
            {
                return new List<AlertShortInfoViewModel>();
            }

            return _alertRepository
                .GetUnseedAlerts(userId.Value);
        }

        public void MarkAsReaded(int alertId)
        {
            var user = _authService.GetCurrentUser();
            _alertRepository.MarkAsReaded(alertId, user);
        }
    }
}
