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
        void Use<T>(string connectionString) where T : class, IMigrationContext;
    }
}
