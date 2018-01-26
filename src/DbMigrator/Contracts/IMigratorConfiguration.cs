using AltaDigital.DbMigrator.Configurations;

namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Default interface for DbMigrator configuring.
    /// </summary>
    public interface IMigratorConfiguration
    {
        /// <summary>
        /// Sets migration context to use.
        /// </summary>
        /// <typeparam name="T">Type of migration context implementation</typeparam>
        void Use<T>(DbConnectionConfig config) where T : class, IMigrationContext;
    }
}
