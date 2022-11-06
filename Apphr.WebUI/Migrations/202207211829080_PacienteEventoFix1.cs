namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteEventoFix1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PacienteEventoHistorials", "CamaOrigen", c => c.Int());
            AddColumn("dbo.PacienteEventoHistorials", "CamaDestino", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PacienteEventoHistorials", "CamaDestino");
            DropColumn("dbo.PacienteEventoHistorials", "CamaOrigen");
        }
    }
}
