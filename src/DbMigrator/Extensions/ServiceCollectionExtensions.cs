using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltaDigital.DbMigrator.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AltaDigital.DbMigrator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Func<Type, bool> IsMigration = 
            type => typeof(IMigration).IsAssignableFrom(type) && type.IsAbstract == false;

        public static IServiceCollection AddDbMigrator(this IServiceCollection services, Action<IMigratorConfiguration> options, Assembly migrationsAssembly = null)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            IEnumerable<Type> migrationTypes = migrationsAssembly == null
                ? AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(IsMigration)
                : migrationsAssembly.GetTypes().Where(IsMigration);

            foreach (Type type in migrationTypes)
            {
                services.AddSingleton(typeof(IMigration), type);
            }
            
            var cfg = new MigrationConfiguration();
            options(cfg);

            services.AddSingleton(cfg.ContextConfig);
            services.AddSingleton(typeof(IMigrationContext), cfg.ContextType);
            services.AddTransient<IMigrator, Migrator>();

            return services;
        }
    }
}
