using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AltaDigital.DbMigrator.Extensions
{
    /// <summary>
    /// Extensions for web-application pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Default way for using DbMigrator.
        /// </summary>
        /// <param name="app">Application builder pipeline</param>
        /// <param name="action">Migrator's action</param>
        public static void UseDbMigrator(this IApplicationBuilder app, Action<IMigrator> action)
        {
            var migrator = app.ApplicationServices.GetService<IMigrator>();
            action(migrator);
        }
    }
}
