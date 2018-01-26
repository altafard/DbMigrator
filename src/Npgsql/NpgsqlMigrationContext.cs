using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using AltaDigital.DbMigrator.Configurations;
using AltaDigital.DbMigrator.Npgsql.Resources;
using Npgsql;

namespace AltaDigital.DbMigrator.Npgsql
{
    /// <inheritdoc />
    /// <summary>
    /// Npgsql context for DbMigrator.
    /// </summary>
    public sealed class NpgsqlMigrationContext : IMigrationContext
    {
        private readonly NpgsqlConnection _connection;
        private IDictionary<long, string> _applied;
        
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Migration context not initialized.</exception>
        public IDictionary<long, string> AppliedMigrations
        {
            get
            {
                if (!this.IsInitialized)
                    throw new InvalidOperationException("Migration context must be initialized first");
                return this._applied;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cfg">Configurations for database provider</param>
        /// <exception cref="ArgumentNullException">The passed argument(s) is NULL</exception>
        /// <exception cref="InvalidOperationException">Configuration is NULL or has invalid values</exception>
        public NpgsqlMigrationContext(DbConnectionConfig cfg)
        {
            if (cfg == null)
                throw new ArgumentNullException(nameof(cfg));
            if (string.IsNullOrEmpty(cfg.ConnectionString))
                throw new InvalidOperationException("Connection string for database cannot bu NULL or empty");

            this._connection = new NpgsqlConnection(cfg.ConnectionString);
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Migration context already has been initialized</exception>
        /// <exception cref="DbException">Some errors occurred on working with DB</exception>
        public async Task Init()
        {
            if (this.IsInitialized)
                throw new InvalidOperationException("Migration context already has been initialized");

            await this._connection.OpenAsync();

            bool exist = await this.MigrationsTableExist();
            if (!exist) await this.CreateMigrationsTable();
            this._applied = await this.GetAppliedMigrations();

            this._connection.Close();

            this.IsInitialized = true;
        }

        /// <inheritdoc />
        /// <param name="migrations">Migrations to include</param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary</exception>
        public async Task Include(IEnumerable<IMigration> migrations)
        {
            foreach (IMigration migration in migrations)
            {
                Type type = migration.GetType();
                await this.InsertMigration(migration.Key, type.Name);
                this._applied.Add(migration.Key, type.Name);
            }
        }

        /// <inheritdoc />
        /// <param name="migrations">Migrations for exclude</param>
        public async Task Exclude(IEnumerable<IMigration> migrations)
        {
            foreach (IMigration migration in migrations)
            {
                await this.DeleteMigration(migration.Key);
                this._applied.Remove(migration.Key);
            }
        }

        /// <inheritdoc />
        /// <param name="sql">SQL</param>
        public void ExecuteSql(string sql)
        {
            this._connection.Open();
            using (NpgsqlCommand command = this._connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            this._connection.Close();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._connection.Dispose();
        }

        #region ' Privates methods '

        private async Task<bool> MigrationsTableExist()
        {
            using (NpgsqlCommand cmd = this._connection.CreateCommand())
            {
                cmd.CommandText = Sql.CheckExisting;
                object result = await cmd.ExecuteScalarAsync();
                return result != null;
            }
        }

        private async Task CreateMigrationsTable()
        {
            using (NpgsqlCommand cmd = this._connection.CreateCommand())
            {
                cmd.CommandText = Sql.Init;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task<Dictionary<long, string>> GetAppliedMigrations()
        {
            var dic = new Dictionary<long, string>();
            using (NpgsqlCommand cmd = this._connection.CreateCommand())
            {
                cmd.CommandText = Sql.Select;

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            long key = reader.GetInt64(0);
                            string type = reader.GetString(1);
                            dic.Add(key, type);
                        }
                    }

                    return dic;
                }
            }
        }

        private async Task InsertMigration(long key, string name)
        {
            await this._connection.OpenAsync();
            using (NpgsqlCommand cmd = this._connection.CreateCommand())
            {
                cmd.CommandText = string.Format(Sql.Insert, key, DateTime.Now.ToString("O"), name);
                await cmd.ExecuteNonQueryAsync();
            }
            this._connection.Close();
        }

        private async Task DeleteMigration(long key)
        {
            await this._connection.OpenAsync();
            using (NpgsqlCommand cmd = this._connection.CreateCommand())
            {
                cmd.CommandText = string.Format(Sql.Delete, key);
                await cmd.ExecuteNonQueryAsync();
            }
            this._connection.Close();
        }

        #endregion
    }
}
