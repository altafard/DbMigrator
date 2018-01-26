namespace AltaDigital.DbMigrator
{
    /// <summary>
    /// Migration actions.
    /// </summary>
    public interface IMigrationAction
    {
        /// <summary>
        /// Execute SQL query.
        /// </summary>
        /// <param name="sql">SQL-script</param>
        void ExecuteSql(string sql);
    }
}
