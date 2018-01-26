using System;

namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Migrations interface.
    /// </summary>
    public interface IMigration : IComparable<IMigration>
    {
        /// <summary>
        /// Unique migration's key.
        /// </summary>
        long Key { get; }

        /// <summary>
        /// Adding new feature in database.
        /// </summary>
        /// <param name="action">Migration actions</param>
        void Up(IMigrationAction action);

        /// <summary>
        /// Downgrades database state.
        /// </summary>
        /// <param name="action">Migration actions</param>
        void Down(IMigrationAction action);
    }
}
