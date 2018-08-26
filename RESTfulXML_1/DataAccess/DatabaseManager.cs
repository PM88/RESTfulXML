using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Configuration;
using System.Data.SqlClient;

namespace RESTfulXML_1.DataAccess
{
    public class DatabaseManager
    {
        private ConnectionStringSettings _configurationManager { get; set; }

        public DatabaseManager()
        {
            _configurationManager = ConfigurationManager.ConnectionStrings["ApplicationContext"];
        }

        public void ResetDatabase()
        {
            DropDatabase(_configurationManager);

            var initializer = new MigrateDatabaseToLatestVersion<ApplicationContext, Migrations.Configuration>();

            Database.SetInitializer(initializer);

            using (var domainContext = new ApplicationContext())
            {
                domainContext.Database.Initialize(true);
            }
        }

        public void UpdateDatabase()
        {
            var migrationConfiguration = new Migrations.Configuration();
            var migrator = new DbMigrator(migrationConfiguration);

            migrator.Update();
        }

        private static void DropDatabase(ConnectionStringSettings connectionString)
        {
            const string DropDatabaseSql =
            "if (select DB_ID('{0}')) is not null\r\n"
            + "begin\r\n"
            + "alter database [{0}] set offline with rollback immediate;\r\n"
            + "alter database [{0}] set online;\r\n"
            + "drop database [{0}];\r\n"
            + "end";

            try
            {
                using (var connection = new SqlConnection(connectionString.ConnectionString))
                {
                    connection.Open();

                    var sqlToExecute = String.Format(DropDatabaseSql, connection.Database);

                    var command = new SqlCommand(sqlToExecute, connection);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw;
            }
        }
    }
}