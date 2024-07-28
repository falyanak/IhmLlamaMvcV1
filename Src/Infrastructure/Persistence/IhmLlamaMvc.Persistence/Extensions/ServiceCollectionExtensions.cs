using System.Globalization;
using IhmLlamaMvc.Application.Interfaces.Dal;
using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Persistence.Constants;
using IhmLlamaMvc.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiccrfCacheManager.Nuget.Interfaces;
using SiccrfCacheManager.Nuget.Services;
using SiccrfDataAccess.Nuget.DAL;
using SiccrfDataAccess.Nuget.Interfaces;

namespace IhmLlamaMvc.Persistence.Extensions;

/// <summary>
/// Extension de la classe services pour isoler la configuration SICCRF
/// </summary>
public static class ServiceCollectionExtensions
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, Serilog.ILogger logger)
    {
        string? connectionString = configuration[Constantes.connectionString];

        // injection de DapperDataAccess pour appeler les procédures stockées via Dapper
        services.AddScoped<IDataAccessService, DapperDataAccessService>(
            provider => new DapperDataAccessService(
                connectionString ?? throw new InvalidOperationException("La chaine de connexion est nulle!")));

    
        // injection du service de statistiques du cache mis en cache
        services.AddScoped<ICacheStatisticService, CacheStatisticService>();

   
        // injection du repository Agent
        services.AddScoped<IAgentRepository, AgentRepository>();

        // injection du repository ModeleIA
        services.AddScoped<IModelIARepository, ModelIARepository>();

        // injection du repository Conversation
        services.AddScoped<IConversationRepository, ConversationRepository>();

    }

    /// <summary>
    /// Mise en cache des données permanentes
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static async Task BuildPermanentCachedDataAsync(this IApplicationBuilder app, 
        Serilog.ILogger logger)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                logger.Information(
                    string.Format("La culture courante est {0} \n\n", CultureInfo.CurrentCulture.Name));

                var permanentCachedDataService = services.GetRequiredService<IPermanentCachedDataService>();

                // mise en cache des listes permanentes  : liste des termes, synonymes
                await permanentCachedDataService.BuildAllPermanentCachedData();

                logger.Information($"Les références techniques éventuelles et les listes immuables ont été mises en cache de manière permanente\n");
            }
            catch (Exception ex)
            {
                logger.Error(
                    $"Une exception a été levée à l'initialisation " +
                    $"du cache à {DateTime.Now} : {ex.InnerException}\n");
                throw;
            }
        }

    }
}