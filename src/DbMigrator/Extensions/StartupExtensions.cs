using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltaDigital.DbMigrator.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AltaDigital.DbMigrator.Extensions
{
    /// <summary>
    /// Extensions for confuguring .NET Core DI and web-application pipeline.
    /// </summary>
    public static class StartupExtensions
    {
        private static readonly Func<Type, bool> IsMigration = 
            type => typeof(IMigration).IsAssignableFrom(type) && type.IsAbstract == false;

        /// <summary>
        /// Injecting DbMigrator and its dependencies to services collection.
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="options">Migration configuration</param>
        /// <param name="migrationsAssembly">Assambly with migrations</param>
        public static IServiceCollection AddDbMigrator(this IServiceCollection services, Action<IMigratorConfiguration> options, Assembly migrationsAssembly = null)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            IEnumerable<Type> migrationTypes = migrationsAssembly == null
                ? AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(IsMigration)
                : migrationsAssembly.GetTypes().Where(IsMigration);

            foreach (Type type in migrationTypes)
            {
                services.AddTransient(type);
            }
            
            var cfg = new MigrationConfiguration();
            options(cfg);

            services.AddSingleton(cfg.ContextConfig);
            services.AddTransient(typeof(IMigrationContextFactory<>), typeof(MigrationContextFactory<>));
            services.AddTransient(typeof(IMigrator<>), typeof(Migrator<>));

            return services;
        }

        /// <summary>
        /// Default way for using DbMigrator.
        /// </summary>
        /// <param name="app">Application builder pipeline</param>
        /// <param name="action">Migrator's action</param>
        public static void UseDbMigrator<TContext>(this IApplicationBuilder app, Action<IMigrator<TContext>> action) where TContext : IMigrationContext
        {
            var migrator = app.ApplicationServices.GetService<IMigrator<TContext>>();
            action(migrator);
        }
    }
}
