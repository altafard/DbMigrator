using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltaDigital.DbMigrator.Exceptions;

namespace AltaDigital.DbMigrator.Core
{
    /// <inheritdoc />
    public class Migrator<TContext> : IMigrator<TContext> where TContext : IMigrationContext
    {
        private readonly IEnumerable<IMigration> _migrations;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="contextFactory">The factory for migration context</param>
        /// <param name="migrations">Migration's array</param>
        /// <exception cref="ArgumentNullException">The passed argument(s) is NULL</exception>
        public Migrator(IMigrationContextFactory<TContext> contextFactory, IEnumerable<IMigration> migrations)
        {
            _migrations = migrations ?? throw new ArgumentNullException(nameof(migrations));

            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        /// <summary>
        /// The factory for migration context.
        /// </summary>
        protected IMigrationContextFactory<TContext> ContextFactory { get; }

        /// <inheritdoc />
        public async Task MigrateAsync()
        {
            await Migrate(migrations => migrations);
        }

        /// <inheritdoc />
        public async Task MigrateAsync(long key, bool ignoreLower = false)
        {
            await Migrate(migrations => migrations.Where(p => !ignoreLower && p.Key <= key || ignoreLower && p.Key == key));
        }

        /// <inheritdoc />
        public async Task MigrateAsync(IMigration migration, bool ignoreLower = false)
        {
            await Migrate(migrations => migrations.Where(p => !ignoreLower && p.Key <= migration.Key || ignoreLower && p == migration));
        }

        /// <inheritdoc />
        public async Task MigrateAsync(IMigration[] migrationsArray)
        {
            await Migrate(migrations => migrations.Intersect(migrationsArray));
        }

        /// <inheritdoc />
        public async Task DowngradeAsync(long key, bool ignoreTop = false)
        {
            await Downgrade(migrations => migrations.Where(p => !ignoreTop && p.Key >= key || ignoreTop && p.Key == key));
        }

        /// <inheritdoc />
        public async Task DowngradeAsync(IMigration migration, bool ignoreTop = false)
        {
            await Downgrade(migrations => migrations.Where(p => !ignoreTop && p.Key >= migration.Key || ignoreTop && p == migration));
        }

        /// <inheritdoc />
        public async Task DowngradeAsync(IMigration[] migrationsArray)
        {
            await Downgrade(migrations => migrations.Intersect(migrationsArray));
        }

        private async Task Migrate(Func<IEnumerable<IMigration>, IEnumerable<IMigration>> statement)
        {
            using (IMigrationContext context = ContextFactory.Create())
            {
                if (context.IsInitialized == false && await context.InitAsync() == false)
                {
                    throw new MigrationContextException("Cannot initialize migration context.");
                }

                IMigration[] migrations = statement(_migrations.Where(migration => context.AppliedMigrations.ContainsKey(migration.Key) == false))
                    .OrderBy(k => k.Key)
                    .ToArray();

                foreach (IMigration migration in migrations)
                {
                    await migration.UpAsync(context);
                }

                await context.IncludeAsync(migrations);
            }
        }

        private async Task Downgrade(Func<IEnumerable<IMigration>, IEnumerable<IMigration>> statement)
        {
            using (IMigrationContext context = ContextFactory.Create())
            {
                if (context.IsInitialized == false && await context.InitAsync() == false)
                {
                    throw new MigrationContextException("Cannot initialize migration context.");
                }

                IEnumerable<IMigration> migrations = statement(_migrations.Where(p => context.AppliedMigrations.ContainsKey(p.Key)))
                    .OrderByDescending(m => m.Key)
                    .ToArray();

                foreach (IMigration migration in migrations)
                {
                    await migration.DownAsync(context);
                }

                await context.ExcludeAsync(migrations);
            }
        }
    }
}
