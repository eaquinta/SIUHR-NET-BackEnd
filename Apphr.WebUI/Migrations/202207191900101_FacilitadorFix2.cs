namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacilitadorFix2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facilitadores",
                c => new
                    {
                        FacilitadorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FacilitadorId)
                .ForeignKey("dbo.Personas", t => t.FacilitadorId)
                .Index(t => t.FacilitadorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facilitadores", "FacilitadorId", "dbo.Personas");
            DropIndex("dbo.Facilitadores", new[] { "FacilitadorId" });
            DropTable("dbo.Facilitadores");
        }
    }
}
