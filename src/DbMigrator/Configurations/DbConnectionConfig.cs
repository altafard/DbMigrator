using System;

namespace AltaDigital.DbMigrator.Configurations
{
    /// <summary>
    /// Configurations for connection to database.
    /// </summary>
    public class DbConnectionConfig
    {
        /// <summary>
        /// Connection string to provider database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public DbConnectionConfig(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be NULL or empty", nameof(connectionString));

            this.ConnectionString = connectionString;
        }
    }
}
