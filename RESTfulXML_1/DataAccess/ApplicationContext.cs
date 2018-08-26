using System.Data.Entity;
using RESTfulXML_1.Models;
using RESTfulXML_1.DataAccess.EntityConfigurations;

namespace RESTfulXML_1.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("name=ApplicationContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationContext, Migrations.Configuration>());
        }

        public virtual DbSet<Request> Requests { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RequestConfiguration());
        }
    }
}