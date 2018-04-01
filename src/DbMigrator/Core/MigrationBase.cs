using System.Threading.Tasks;

namespace AltaDigital.DbMigrator.Core
{
    /// <summary>
    /// Base migration implementation with defined comparator.
    /// </summary>
    public abstract class MigrationBase : IMigration
    {
        /// <inheritdoc />
        public abstract long Key { get; }

        /// <inheritdoc />
        public abstract Task UpAsync(IMigrationAction action);

        /// <inheritdoc />
        public abstract Task DownAsync(IMigrationAction action);

        /// <inheritdoc />
        /// <param name="other">Migration for compare</param>
        public int CompareTo(IMigration other)
        {
            if (this.Key > other.Key)
            {
                return 1;
            }

            if (this.Key < other.Key)
            {
                return -1;
            }

            return 0;
        }
    }
}
