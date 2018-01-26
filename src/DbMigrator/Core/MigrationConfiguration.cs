using System;
using AltaDigital.DbMigrator.Configurations;

namespace AltaDigital.DbMigrator.Core
{
    /// <summary>
    /// Configurator of migration context for DI.
    /// </summary>
    internal sealed class MigrationConfiguration : IMigratorConfiguration
    {
        /// <summary>
        /// Type of migration context implementation.
        /// </summary>
        public Type ContextType { get; private set; }

        public DbConnectionConfig ContextConfig { get; private set; }

        /// <inheritdoc />
        /// <typeparam name="T">Type of migration context implementation</typeparam>
        public void Use<T>(DbConnectionConfig config) where T : class, IMigrationContext
        {
            Type type = typeof(T);
            if (type.IsAbstract)
                throw new ArgumentException("Configurator must get implementation of interface IMigrationContext", nameof(T));

            this.ContextType = type;
            this.ContextConfig = config;
        }
    }
}