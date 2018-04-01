using AltaDigital.DbMigrator.Configurations;

namespace AltaDigital.DbMigrator.Core
{
    /// <summary>
    /// Configurator of migration context for DI.
    /// </summary>
    internal sealed class MigrationConfiguration : IMigratorConfiguration
    {
        public MigrationContextConfig ContextConfig { get; private set; }

        /// <inheritdoc />
        public void Use<T>(string connectionString, bool ensureDatabaseExists = true) where T : IMigrationContext
        {
            this.ContextConfig = new MigrationContextConfig(connectionString, ensureDatabaseExists);
        }
    }
}