using IhmLlamaMvc.Application.Configurations;
using IhmLlamaMvc.Mvc.Constants;
using IhmLlamaMvc.Mvc.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReferentielAPI.Entites;
using SiccrfAuthorization.Nuget.Interfaces;

namespace IhmLlamaMvc.Mvc.Controllers;

public class BaseController : Controller
{
    protected readonly ISiccrfAuthorizationService _siccrfAuthorizationService;
    protected readonly ILogger<BaseController> _logger;
    protected readonly ApplicationSettings _applicationSettings;
    protected readonly ISender _sender;

    public BaseController(
        ISiccrfAuthorizationService siccrfAuthorizationService,
        ILogger<BaseController> logger,
        IOptions<ApplicationSettings> applicationSettings,
        ISender sender)
    {
        _siccrfAuthorizationService = siccrfAuthorizationService;
        _logger = logger;
        _applicationSettings = applicationSettings.Value;
        _sender = sender;
    }
    protected async Task<AgentPermissions> GetAgentPermissions() => 
        await _siccrfAuthorizationService.GetAgentAsync();

    protected async Task<AgentPermissions> GetInfosAgent()
    {
        var agentPermissions = await GetAgentPermissions();

        HttpContext.Session.SetJson<AgentPermissions>(
            Constantes.SessionKeyInfosUser, agentPermissions);

        return agentPermissions;

    }
}