using System.Threading.Tasks;

namespace AltaDigital.DbMigrator
{

    /// <summary>
    /// Migrator for upgrading and downgrading database state.
    /// </summary>
    /// <typeparam name="TContext">Type of migration context</typeparam>
    public interface IMigrator<out TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// Migrate to last migration.
        /// </summary>
        Task MigrateAsync();

        /// <summary>
        /// Migrate to migration with given key.
        /// </summary>
        /// <param name="key">Migration key</param>
        /// <param name="ignoreLower">Try to migrate only the given migration without migrating lower migrations</param>
        Task MigrateAsync(long key, bool ignoreLower = false);

        /// <summary>
        /// Migrate to given migration.
        /// </summary>
        /// <param name="migration">Migration for applying</param>
        /// <param name="ignoreLower">Try to migrate only the given migration without migrating lower migrations</param>
        Task MigrateAsync(IMigration migration, bool ignoreLower = false);

        /// <summary>
        /// Consistently applies each migration.
        /// </summary>
        /// <param name="migrations">Migrations for applying</param>
        Task MigrateAsync(IMigration[] migrations);

        /// <summary>
        /// Downgrade to migration with key.
        /// </summary>
        /// <param name="key">Migration key</param>
        /// <param name="ignoreUpper">Try to downgrade only the given migration without downgrading top migrations</param>
        Task DowngradeAsync(long key, bool ignoreUpper = false);

        /// <summary>
        /// Downgrade given migration.
        /// </summary>
        /// <param name="migration">Migration for downgrade</param>
        /// <param name="ignoreUpper">Try to downgrade only the given migration without downgrading top migrations</param>
        Task DowngradeAsync(IMigration migration, bool ignoreUpper = false);

        /// <summary>
        /// Consistently downgrades each migration.
        /// </summary>
        /// <param name="migrations">Migrations for downgrade</param>
        Task DowngradeAsync(IMigration[] migrations);
    }
}
