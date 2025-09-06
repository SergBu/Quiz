using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharpQuiz.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContextPool<DatabaseContext>(optionsAction =>
        {
            optionsAction.EnableSensitiveDataLogging();
            optionsAction.UseNpgsql(connectionString,
                x => { x.MigrationsHistoryTable("MigrationsHistory", DatabaseContext.SchemaName); });
        });
        return services;
    }
}