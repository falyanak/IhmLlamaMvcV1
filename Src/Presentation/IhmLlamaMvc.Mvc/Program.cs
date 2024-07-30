using IhmLlamaMvc.Application.Extensions;
using IhmLlamaMvc.Mvc.Extensions;
using IhmLlamaMvc.Mvc.Middleware;
using IhmLlamaMvc.Persistence.EF;
using IhmLlamaMvc.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Configuration;

// Logger pour la phase de build dans un fichier dédié
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("starting server.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews();
    //.AddNewtonsoftJson(o => o.SerializerSettings
    //    .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


    // SonarQube ajout SSL/TLS
    // authentification
    // Add services to the container.
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
    //   builder.Services.AddAuthentication();

    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy.
        options.FallbackPolicy = options.DefaultPolicy;
    });

    builder.Services.AddRazorPages();


    // installation Serilog
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    // Injecter les services d'infrastructure de l'application
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration, Log.Logger);

    //*********************** SICCRF IhmLlamaMvcV1 Configuration *******************************


    // Add services to the container.

    // gestion des sessions
    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = ".IhmLlamaMvcV1.Session";
        // ce paramètre est essentiel pour utiliser le cache pour Siccrf.Authorization
        options.Cookie.IsEssential = true;
        // IdleTimeout indique la durée pendant laquelle la session peut être inactive
        // avant que son contenu soit abandonné.
        // Chaque accès à la session réinitialise le délai d’expiration.
        // Ce paramètre s’applique uniquement au contenu de la session, et non au cookie.
        options.IdleTimeout = TimeSpan.FromMinutes(30);
    });

    // Add a database provider (import the Microsoft.EntityFrameworkCore namespace!)
    builder.Services.AddDbContext<ChatIaDbContext>(options =>
    {
        // avec plusieurs chaines connections
        var connectionString =
            builder.Configuration["ApplicationSettings:ConnectionStrings:DefaultConnection2"]
             ?? throw new InvalidOperationException(
                "Chaine de connexion à la base de données non trouvée !");

        // avec une seule chaine de connexion
        //var connectionString = builder.Configuration
        //    .GetConnectionString("DefaultConnection")
        //      ?? throw new InvalidOperationException("Chaine de connexion à la base de données non trouvée !");

        options.UseSqlServer(connectionString);
    }, ServiceLifetime.Singleton);

    var app = builder.Build();

    //*********************** SICCRF ProfilPlus Configuration *******************************
    //await app.BuildPermanentCachedDataAsync(Log.Logger);
    //*********************** SICCRF ProfilPlus Configuration *******************************


    // Configure the HTTP request pipeline.
    app.UseMiddleware<CustomExceptionHandlerMiddleware>();

    // https://kendaleiv.com/setting-regex-timeout-globally-using-dotnet-6_0-with-csharp/
    // Regular expressions could be used by an attacker to launch
    // a denial-of-service attack for a website by consuming excessive resources.
    // Setting a timeout allows the operation to stop at a configured timeout,
    // rather than running until completion, using resources the entire time.
    // configuration du timeout pour toutes les expressions régulières

    //AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(2));

    // ligne à activer pour tracer les requêtes HTTP
    //  app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseCookiePolicy();
    app.UseSession();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=ShowIaPrompt}/{id?}");


    // migration
    //await using var scope = app.Services.CreateAsyncScope();
    //await using var db = scope.ServiceProvider.GetService<ChatIaContext>();
    //await db.Database.MigrateAsync();

    app.Run();

    Log.Information("L'application a été configurée et lancée.");
}

catch (Exception ex)
{
    Log.Fatal(ex, "Fin inattendue de la phase de démarrage !");
}
finally
{
    Log.CloseAndFlush();
}

