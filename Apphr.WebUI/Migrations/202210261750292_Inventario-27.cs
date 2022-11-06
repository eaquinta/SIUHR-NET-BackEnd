namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SolicitudDespacho", "Observacion", c => c.String());
            AddColumn("dbo.SolicitudDespacho", "Lineas", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "DespachoInventarioId", c => c.String(maxLength: 15));
            AddColumn("dbo.SolicitudDespacho", "HojaControlSala", c => c.String(maxLength: 10));
            AddColumn("dbo.SolicitudDespacho", "FechaOperacion", c => c.DateTime());
            AddColumn("dbo.SolicitudDespacho", "Servicio", c => c.String(maxLength: 20));
            AddColumn("dbo.SolicitudDespacho", "Cama", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "PacienteId", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "Cirujano", c => c.String(maxLength: 100));
            AddColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaInstrumentista", c => c.String(maxLength: 100));
            AddColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaCirculante", c => c.String(maxLength: 100));
            CreateIndex("dbo.SolicitudDespacho", "PacienteId");
            AddForeignKey("dbo.SolicitudDespacho", "PacienteId", "dbo.Pacientes", "PacienteId");
            DropColumn("dbo.SolicitudDespacho", "Observaciones");
            DropColumn("dbo.SolicitudDespacho", "NumeroLineas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudDespacho", "NumeroLineas", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "Observaciones", c => c.String());
            DropForeignKey("dbo.SolicitudDespacho", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.SolicitudDespacho", new[] { "PacienteId" });
            DropColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaCirculante");
            DropColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaInstrumentista");
            DropColumn("dbo.SolicitudDespacho", "Cirujano");
            DropColumn("dbo.SolicitudDespacho", "PacienteId");
            DropColumn("dbo.SolicitudDespacho", "Cama");
            DropColumn("dbo.SolicitudDespacho", "Servicio");
            DropColumn("dbo.SolicitudDespacho", "FechaOperacion");
            DropColumn("dbo.SolicitudDespacho", "HojaControlSala");
            DropColumn("dbo.SolicitudDespacho", "DespachoInventarioId");
            DropColumn("dbo.SolicitudDespacho", "Lineas");
            DropColumn("dbo.SolicitudDespacho", "Observacion");
        }
    }
}
