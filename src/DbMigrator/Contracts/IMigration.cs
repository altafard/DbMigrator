using System;
using System.Threading.Tasks;

namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Migration interface.
    /// </summary>
    public interface IMigration : IComparable<IMigration>
    {
        /// <summary>
        /// Unique migration's key.
        /// </summary>
        /// <remarks>It recommended to use unique serial key. For example timestamp of creation the migration in format 'yyyyMMddHHmmss'</remarks>
        long Key { get; }

        /// <summary>
        /// Adding new feature in database.
        /// </summary>
        /// <param name="action">Migration actions</param>
        Task UpAsync(IMigrationAction action);

        /// <summary>
        /// Downgrades database state.
        /// </summary>
        /// <param name="action">Migration actions</param>
        Task DownAsync(IMigrationAction action);
    }
}
