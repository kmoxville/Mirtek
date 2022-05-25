using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Quartz;
using RssFeedAggregator.BackgroundJobs;
using RssFeedAggregator.DAL;
using RssFeedAggregator.DAL.UnitOfWork;
using RssFeedAggregator.Services.RssFeedDownloader;
using RssFeedAggregator.Services.RssFeedService;
using RssFeedAggregator.Utils.Options;
using RssFeedAggregator.Validation.Requests.RssFeedRequests;

namespace RssFeedAggregator.Utils
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MySQLOptions>(
                config.GetSection(MySQLOptions.Position));

            services.Configure<GeneralOptions>(
                config.GetSection(GeneralOptions.Position));

            services.Configure<HttpClientOptions>(
                config.GetSection(HttpClientOptions.Position));

            return services;
        }

        public static IServiceCollection AddDatabaseContext(
            this IServiceCollection services, IConfiguration config, bool isDevelopment)
        {
            var options = config.GetSection(MySQLOptions.Position).Get<MySQLOptions>();
            var connectionString = options.GetConnectionString();
            var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));

            services.AddDbContext<DatabaseContext>(
                dbContextOptions =>
                {
                    dbContextOptions.UseMySql(connectionString, serverVersion)
                        .EnableDetailedErrors();

                    if (isDevelopment)
                    {
                        dbContextOptions.LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging();
                    }
                    else
                    {
                        dbContextOptions.LogTo(Console.WriteLine, LogLevel.Warning)
                            .EnableSensitiveDataLogging();
                    }
                });

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IRegisterRequestValidator, RegisterRequestValidator>();
            services.AddScoped<IGetPostsRequestValidator, GetPostsRequestValidator>();

            return services;
        }

        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<IRssFeedService, RssFeedService>();
            services.AddScoped<IRssFeedDownloaderService, RssFeedDownloaderService>();

            return services;
        }

        public static void AddHttpClientRetryPolicy(this IServiceCollection services, IConfiguration config)
        {
            var options = config.GetSection(HttpClientOptions.Position).Get<HttpClientOptions>();

            services.AddHttpClient(Settings.RESILIENT_CLIENT)
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); 
        }

        public static IServiceCollection AddScheduledJobs(
             this IServiceCollection services, IConfiguration config)
        {
            var backgroundJobOptions = config.GetSection(RssFeedPollerOptions.Position).Get<RssFeedPollerOptions>();
            var interval = backgroundJobOptions?.Interval ?? 5;

            services.AddScoped<RssFeedPollerJob>()
                .AddQuartz(cfg =>
                {
                    cfg.UseMicrosoftDependencyInjectionJobFactory();
                    cfg.SchedulerName = "default";
                    cfg.ScheduleJob<RssFeedPollerJob>(trigger =>
                    {
                        trigger.WithIdentity("rss-poller", "background-jobs")
                            .WithDailyTimeIntervalSchedule(sch => sch.WithInterval(interval, IntervalUnit.Second));

                        trigger.StartNow();
                    });
                });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
