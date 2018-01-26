using System.Threading.Tasks;

namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Migrator for upgrading and downgrading database state.
    /// </summary>
    public interface IMigrator
    {
        /// <summary>
        /// Migrate to last migration.
        /// </summary>
        Task Migrate();

        /// <summary>
        /// Migrate to migration with key.
        /// </summary>
        /// <param name="key">Migration identifier</param>
        Task Downgrade(long key);
    }
}
