using System.Reflection;

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
        /// <param name="connectionString">Connection string to database</param>
        /// <param name="ensureDatabaseExists">Ensure database exists</param>
        void Use<T>(string connectionString, bool ensureDatabaseExists = true) where T : IMigrationContext;

        /// <summary>
        /// Sets assembly for search of migrations.
        /// </summary>
        /// <param name="assembly">Migrations assembly</param>
        void SetMigrationsAssembly(Assembly assembly);
    }
}
