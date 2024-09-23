using Application.Dtos.UseCases;
using Application.Mappings.UseCases;
using Application.UseCases;
using Application.Validation.Validators;
using Core.Interfaces.Repository;
using Domain.Interfaces.DbContext;
using FluentValidation;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Runtime.CompilerServices;

IHost host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        _ = services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            OpenApiConfigurationOptions options = new()
            {
                Info = new OpenApiInfo()
                {
                    Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                    Title = $"{DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle()}",
                    Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription()
                },
                Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                OpenApiVersion = DefaultOpenApiConfigurationOptions.GetOpenApiVersion(),
                IncludeRequestingHostName = DefaultOpenApiConfigurationOptions.IsFunctionsRuntimeEnvironmentDevelopment(),
                ForceHttps = DefaultOpenApiConfigurationOptions.IsHttpsForced(),
                ForceHttp = DefaultOpenApiConfigurationOptions.IsHttpForced(),
            };

            return options;
        });

        _ = services.AddDbContext<IParkingPepitoDbContext, ParkingPepitoDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:SQLConnectionString"));
            }, ServiceLifetime.Transient
        );

        //Logger
        _ = services.AddApplicationInsightsTelemetryWorkerService();
        _ = services.ConfigureFunctionsApplicationInsights();
        _ = services.Configure<LoggerFilterOptions>(options =>
        {
            // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
            // Log levels can also be configured using appsettings.json. For more information, see https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#ilogger-logs
            LoggerFilterRule toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

            if (toRemove is not null)
            {
                _ = options.Rules.Remove(toRemove);
            }
        });

        //DbContext
        services.AddScoped<IParkingPepitoDbContext, ParkingPepitoDbContext>();

        //Repository
        _ = services.AddScoped(typeof(IEntitiesRepository<>), typeof(EntitiesRepository<>));

        //UseCases
        _ = services.AddScoped<IRegisterEntryUseCase, RegisterEntryUseCase>();

        //Automapper
        _ = services.AddAutoMapper(typeof(EntryRegisterUseCaseMapping));

        //FluentValidator
        services.AddScoped<IValidator<RegisterEntryUseCaseDto>, RegisterEntryUseCaseDtoValidator>();
    })
    .ConfigureLogging(logging =>
    {
        _ = logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule defaltRule = options.Rules.FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaltRule is not null)
            {
                _ = options.Rules.Remove(defaltRule);
            }
        });
    })
    .Build();

host.Run();
