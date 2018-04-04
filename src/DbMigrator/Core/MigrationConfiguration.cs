using System.Collections.Generic;
using System.Reflection;
using AltaDigital.DbMigrator.Configurations;

namespace AltaDigital.DbMigrator.Core
{
    /// <summary>
    /// Configurator of migration context for DI.
    /// </summary>
    internal sealed class MigrationConfiguration : IMigratorConfiguration
    {
        private readonly List<Assembly> _assemblies = new List<Assembly>();

        public MigrationContextConfig ContextConfig { get; private set; }

        public IReadOnlyList<Assembly> MigrationsAssemblies => _assemblies;

        /// <inheritdoc />
        public void Use<T>(string connectionString, bool ensureDatabaseExists = true) where T : IMigrationContext
        {
            this.ContextConfig = new MigrationContextConfig(connectionString, ensureDatabaseExists);
        }

        public void SetMigrationsAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }
    }
}