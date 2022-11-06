namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteEventoFix2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PacienteEventoHistorials", "DiagnosticoAnterior", c => c.String());
            AddColumn("dbo.PacienteEventoHistorials", "DiagnosticoNuevo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PacienteEventoHistorials", "DiagnosticoNuevo");
            DropColumn("dbo.PacienteEventoHistorials", "DiagnosticoAnterior");
        }
    }
}
