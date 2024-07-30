namespace IhmLlamaMvc.Persistence.Constants;

/// <summary>
/// Fichier de centralisation des constantes
/// Objectif : simplifier la maintenance en cas de changement de nom
/// </summary>
public static class Constantes
{
    // Racine de la configuration dans appsettings.json
    //    public const string foodexConfiguration = "FoodexConfiguration";

    // appsettings keys
    public const string connectionString = "ApplicationSettings:ConnectionStrings:DefaultConnection2";
    public const string hasReferenceTechnique = "HasReferenceTechnique";

    public const string cacheConfiguration = "CacheConfiguration";
    public const string cacheConfigurationMonitoring = "CacheConfiguration:Monitoring";

    // Procédures stockées

    public const string spGetUnAgent = "CHAT_IA_S_UN_AGENT";
    public const string SpSupprimerAgent = "CHAT_IA_D_AGENT";
    public const string SpCreerAgent = "CHAT_IA_I_AGENT";
    public const string SpListerAgents = "CHAT_IA_S_AGENTS";



    //     public const string cacheDic = "_cacheDic";
    public const string cacheStatistiques = "_cacheStatistiques";


}