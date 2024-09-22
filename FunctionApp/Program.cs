using Application.Dtos.UseCases;
using Application.UseCases;
using Application.Validation.Validators;
using Core.Interfaces.Repository;
using Domain.Interfaces.DbContext;
using FluentValidation;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Reflection;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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
            options.UseSqlServer(Environment.GetEnvironmentVariable("SQLConnectionString"));
            }
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

        //UseCases
        _ = services.AddScoped<IRegisterEntryUseCase, RegisterEntryUseCase>();

        //Repository
        _ = services.AddScoped(typeof(IEntitiesRepository<>), typeof(EntitiesRepository<>));

        //Automapper
        _ = services.AddAutoMapper(Assembly.GetEntryAssembly());

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
