namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteEventDetalleFix1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PacienteEventoHistorials", "UserId", c => c.Int());
            CreateIndex("dbo.PacienteEventoHistorials", "UserId");
            AddForeignKey("dbo.PacienteEventoHistorials", "UserId", "dbo.AppUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PacienteEventoHistorials", "UserId", "dbo.AppUser");
            DropIndex("dbo.PacienteEventoHistorials", new[] { "UserId" });
            DropColumn("dbo.PacienteEventoHistorials", "UserId");
        }
    }
}
