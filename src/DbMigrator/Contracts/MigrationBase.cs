namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Base migration implementation with defined comparator.
    /// </summary>
    public abstract class MigrationBase : IMigration
    {
        public abstract long Key { get; }

        public abstract void Up(IMigrationAction action);

        public abstract void Down(IMigrationAction action);

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
