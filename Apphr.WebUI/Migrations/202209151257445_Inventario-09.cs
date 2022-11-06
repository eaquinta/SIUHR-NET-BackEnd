namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppDefaults",
                c => new
                    {
                        AppDefaultId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.AppDefaultId)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AppDefaults", new[] { "Name" });
            DropTable("dbo.AppDefaults");
        }
    }
}
