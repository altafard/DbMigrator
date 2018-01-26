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
        IDictionary<long, string> AppliedMigrations { get; }
        
        /// <summary>
        /// Initializes context.
        /// </summary>
        Task Init();

        /// <summary>
        /// Includes migrations to applied list.
        /// </summary>
        /// <param name="migrations">Migrations to include</param>
        Task Include(IEnumerable<IMigration> migrations);

        /// <summary>
        /// Excludes migrations from applied list.
        /// </summary>
        /// <param name="migrations">Migrations for exclude</param>
        Task Exclude(IEnumerable<IMigration> migrations);
    }
}
