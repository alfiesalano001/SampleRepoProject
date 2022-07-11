using System;
using AutoMapper;
using DomainModels.Entity;
using DomainModels.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.DbContexts;
using Repositories.Decorators;
using Repositories.Interface;
using Repositories.Mappings;
using Services;
using Services.Decorators;

namespace ASPNETCoreMasterProj.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add database dependency
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionProfile = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                                                .MustNotBeEmpty($"Connection Profile is invalid. Please configure your connection profile properly.");

            if (connectionProfile.ToLower() == "inmemory")
            {
                services.AddDbContext<SqlDataContext>(_ => _.UseInMemoryDatabase(connectionProfile));
            }
            else
            {
                var connectionString = configuration.GetConnectionString(connectionProfile)
                                                    .MustNotBeEmpty($"Connection String for {connectionProfile} is invalid. Please configure your connection string properly.");

                services.AddDbContext<SqlDataContext>(_ => _.UseSqlServer(connectionString));
            }
        }

        /// <summary>
        /// Add service dependencies
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddDecoratedService<IMenuService, MenuService, MenuServiceDecorator>();
            services.AddScoped<IOrderService, OrderService>();
        }

        /// <summary>
        /// Add repository dependencies
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            //Add DbContexts
            //NOTE: This is intentional as the application is currently using 2 data sources - Json and SQL
            //      Can be simplified when all there is only 1 data source
            services.AddScoped<JsonDataContext>();
            services.AddScoped<SqlDataContext>();
            services.AddScoped<Func<DataSource, IDataContext>>(provider => key =>
            {
                switch (key)
                {
                    case DataSource.Sql:
                        return new DataContextDecorator(
                                provider.GetService<SqlDataContext>(),
                                provider.GetService<ILogger<DataContextDecorator>>());
                    case DataSource.Json:
                        return new DataContextDecorator(
                                provider.GetService<JsonDataContext>(),
                                provider.GetService<ILogger<DataContextDecorator>>());
                    default:
                        throw new NotImplementedException();
                }
            });

            //Register SQL Entities
            services.AddScoped<IOrderRepository, OrderRepository>(provider
                => new OrderRepository(provider.GetService<Func<DataSource, IDataContext>>()(DataSource.Sql)));

            //Regsiter JSON Entities
            services.AddScoped<IMenuRepository, MenuRepository>(provider
                => new MenuRepository(provider.GetService<Func<DataSource, IDataContext>>()(DataSource.Json)));
            services.AddScoped<IGenericRepository<Stock>, GenericRepository<Stock>>(provider
                => new GenericRepository<Stock>(provider.GetService<Func<DataSource, IDataContext>>()(DataSource.Json)));
        }

        /// <summary>
        /// Add mapper configurations
        /// </summary>
        /// <param name="services"></param>
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        /// <summary>
        /// Add a decorated service
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TDecorator"></typeparam>
        /// <param name="services"></param>
        private static void AddDecoratedService<TInterface, TImplementation, TDecorator>(this IServiceCollection services)
        {
            services.AddScoped(typeof(TInterface), typeof(TImplementation));
            services.Decorate(typeof(TInterface), typeof(TDecorator));
        }
    }

    enum DataSource
    {
        Sql,
        Json
    }
}
