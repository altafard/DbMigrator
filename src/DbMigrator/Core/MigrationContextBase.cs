using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltaDigital.DbMigrator.Configurations;
using AltaDigital.DbMigrator.Exceptions;

namespace AltaDigital.DbMigrator.Core
{
    /// <inheritdoc />
    public abstract class MigrationContextBase : IMigrationContext
    {
        private Dictionary<long, string> _appliedMigrations;

        /// <summary>
        /// Dictionary with connection claims.
        /// </summary>
        protected MigrationContextConfig Configuration { get; }

        /// <summary>
        /// Base ctor.
        /// </summary>
        protected MigrationContextBase(MigrationContextConfig config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<long, string> AppliedMigrations
        {
            get
            {
                if (IsInitialized == false)
                    throw new MigrationContextException("Migration context must be initialized first");
                return _appliedMigrations;
            }
        }

        /// <inheritdoc />
        public async Task<bool> InitAsync()
        {
            if (IsInitialized)
                throw new MigrationContextException("Migration context already has been initialized");

            if (Configuration.EnsureDbExists) await EnsureExistDatabaseAsync();
            await EnsureExistMigrationTableAsync();

            _appliedMigrations = await LoadMigrationsAsync();

            return IsInitialized = true;
        }

        /// <inheritdoc />
        public async Task IncludeAsync(IEnumerable<IMigration> migrations)
        {
            foreach (IMigration migration in migrations)
            {
                Type type = migration.GetType();
                await InsertMigrationAsync(migration, type.Name);
                _appliedMigrations.Add(migration.Key, type.Name);
            }
        }

        /// <inheritdoc />
        public async Task ExcludeAsync(IEnumerable<IMigration> migrations)
        {
            foreach (IMigration migration in migrations)
            {
                await RemoveMigrationAsync(migration);
                _appliedMigrations.Remove(migration.Key);
            }
        }

        /// <summary>
        /// Insert migration to database table.
        /// </summary>
        /// <param name="migration">Migration for inserting</param>
        /// <param name="name"></param>
        protected abstract Task InsertMigrationAsync(IMigration migration, string name);
        /// <summary>
        /// Remove migration from database table.
        /// </summary>
        /// <param name="migration">Migration for removing</param>
        protected abstract Task RemoveMigrationAsync(IMigration migration);
        /// <summary>
        /// Ensure exist database.
        /// </summary>
        protected abstract Task EnsureExistDatabaseAsync();
        /// <summary>
        /// Ensure exist migration table.
        /// </summary>
        protected abstract Task EnsureExistMigrationTableAsync();
        /// <summary>
        /// Select existing migrations from database.
        /// </summary>
        protected abstract Task<Dictionary<long, string>> LoadMigrationsAsync();

        /// <inheritdoc />
        public abstract Task ExecuteAsync(string sql);
        /// <inheritdoc />
        public abstract void Dispose();
    }
}
