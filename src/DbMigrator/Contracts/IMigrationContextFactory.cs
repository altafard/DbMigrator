namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Factory for creation of migration context.
    /// </summary>
    /// <typeparam name="TContext">Type of migration context</typeparam>
    public interface IMigrationContextFactory<out TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// Create migration context.
        /// </summary>
        /// <returns>Instance of <see cref="IMigrationContext"/>.</returns>
        TContext Create();
    }
}
