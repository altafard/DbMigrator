using AltaDigital.DbMigrator.Configurations;
using AltaDigital.DbMigrator.Extensions;
using AltaDigital.DbMigrator.Npgsql;
using AltaDigital.DbMigrator.Tests.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AltaDigital.DbMigrator.Tests.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true);

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connString = this.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbMigrator(configuration => configuration
                .Use<NpgsqlMigrationContext>(new DbConnectionConfig(connString)), typeof(Migration1).Assembly);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDbMigrator(async migrator => await migrator.Migrate());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
