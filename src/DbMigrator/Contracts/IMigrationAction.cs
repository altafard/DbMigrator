using System.Threading.Tasks;

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
        Task ExecuteAsync(string sql);
    }
}
