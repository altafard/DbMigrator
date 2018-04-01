using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Migration context for DbMigrator. Implementation represent database context from DB providers.
    /// </summary>
    public interface IMigrationContext : IMigrationAction, IDisposable
    {
        /// <summary>
        /// Indicates whether the context is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Stores applied migrations.
        /// </summary>
        IReadOnlyDictionary<long, string> AppliedMigrations { get; }

        /// <summary>
        /// Initializes context.
        /// </summary>
        /// <returns>Returns TRUE if context initialized, FALSE in other case.</returns>
        Task<bool> InitAsync();

        /// <summary>
        /// Includes migrations to applied list.
        /// </summary>
        /// <param name="migrations">Migrations to include</param>
        Task IncludeAsync(IEnumerable<IMigration> migrations);

        /// <summary>
        /// Excludes migrations from applied list.
        /// </summary>
        /// <param name="migrations">Migrations for exclude</param>
        Task ExcludeAsync(IEnumerable<IMigration> migrations);
    }
}
