using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpQuiz.Database.Extensions;
using SharpQuiz.Database.Repositories.Implementations;
using SharpQuiz.Database.Repositories.Interfaces;


namespace SharpQuiz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        // services.AddSmartSeedsRabbitConfigurationFromLib(configuration);
        // services.AddSmartseedsRabbitPublisher();
        // services.AddSmartseedsRabbitListener();
        // services.AddReportServerPublishers();
        services.AddScoped<IBookRepository, BookRepository>();
        // services.AddScoped<JobHelper>();
        // services.AddScoped<IJobGenerateReportService, JobGenerateReportService>();

        return services;
    }
}