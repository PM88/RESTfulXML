namespace RESTfulXML_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Ix = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Visits = c.Int(),
                    })
                .PrimaryKey(t => new { t.Ix, t.Name, t.Date });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Requests");
        }
    }
}
