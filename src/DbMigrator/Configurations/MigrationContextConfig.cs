using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AltaDigital.DbMigrator.Exceptions;

namespace AltaDigital.DbMigrator.Configurations
{
    /// <summary>
    /// Configurations for connection to database.
    /// </summary>
    public class MigrationContextConfig
    {
        /// <summary>
        /// Connection string to provider database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Ensure database exists.
        /// </summary>
        public bool EnsureDbExists { get; }

        /// <summary>
        /// Connection claims from connection string.
        /// </summary>
        public IReadOnlyDictionary<string, string> ConnectionClaims => ParseConnectionClaims();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString">Connection string to database</param>
        /// <param name="ensureDatabaseExists">Ensure database exists</param>
        public MigrationContextConfig(string connectionString, bool ensureDatabaseExists)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be NULL or empty", nameof(connectionString));

            ConnectionString = connectionString;
            EnsureDbExists = ensureDatabaseExists;
        }

        /// <summary>
        /// Parse claims from connection string.
        /// </summary>
        /// <returns>Dictionary with connection claims</returns>
        private Dictionary<string, string> ParseConnectionClaims()
        {
            var claims = new Dictionary<string, string>();

            var regex = new Regex("(?<Key>[^=;]+)=(?<Value>[^;]+)");
            if (regex.IsMatch(ConnectionString) == false) throw new MigrationContextException("Invalid connection string");
            MatchCollection matches = regex.Matches(ConnectionString);

            foreach (Match match in matches)
            {
                claims.Add(match.Groups["Key"].Value, match.Groups["Value"].Value);
            }

            return claims;
        }
    }
}
