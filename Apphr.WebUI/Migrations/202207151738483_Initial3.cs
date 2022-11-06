namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUser", "PersonaId", c => c.Int());
            CreateIndex("dbo.AppUser", "PersonaId");
            AddForeignKey("dbo.AppUser", "PersonaId", "dbo.Personas", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUser", "PersonaId", "dbo.Personas");
            DropIndex("dbo.AppUser", new[] { "PersonaId" });
            DropColumn("dbo.AppUser", "PersonaId");
        }
    }
}
