using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaDigital.DbMigrator.Core
{
    /// <inheritdoc />
    public class Migrator : IMigrator
    {
        private readonly IMigrationContext _context;
        private readonly IEnumerable<IMigration> _migrations;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Migration context</param>
        /// <param name="migrations">Migration's array</param>
        /// <exception cref="ArgumentNullException">The passed argument(s) is NULL</exception>
        public Migrator(IMigrationContext context, IEnumerable<IMigration> migrations)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._migrations = migrations ?? throw new ArgumentNullException(nameof(migrations));
        }

        /// <inheritdoc />
        public async Task Migrate()
        {
            if (this._context.IsInitialized == false)
                await this._context.Init();

            IEnumerable<IMigration> migrations = this._migrations
                .Where(migration => this._context.AppliedMigrations.ContainsKey(migration.Key) == false)
                .OrderBy(k => k.Key)
                .ToArray();

            foreach (IMigration migration in migrations)
            {
                migration.Up(this._context);
            }

            await this._context.Include(migrations);
        }

        /// <inheritdoc />
        /// <param name="key">Migration identifier</param>
        public async Task Downgrade(long key)
        {
            if (this._context.IsInitialized == false)
                await this._context.Init();

            IEnumerable<IMigration> migrations = this._migrations
                .Where(m => m.Key > key)
                .OrderByDescending(m => m.Key)
                .ToArray();

            foreach (IMigration migration in migrations)
            {
                migration.Down(this._context);
            }

            await this._context.Exclude(migrations);
        }
    }
}
