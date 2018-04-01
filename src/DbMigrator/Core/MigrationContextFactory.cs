using System;
using AltaDigital.DbMigrator.Configurations;

namespace AltaDigital.DbMigrator.Core
{
    /// <inheritdoc />
    public class MigrationContextFactory<TContext> : IMigrationContextFactory<TContext> where TContext : IMigrationContext
    {
        private readonly MigrationContextConfig _config;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="config">Database configuration</param>
        public MigrationContextFactory(MigrationContextConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc />
        public TContext Create()
        {
            return (TContext) Activator.CreateInstance(typeof(TContext), _config);
        }
    }
}
