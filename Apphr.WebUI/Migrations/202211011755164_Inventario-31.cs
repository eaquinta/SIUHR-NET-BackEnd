namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario31 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudDespacho", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.SolicitudDespacho", new[] { "PacienteId" });
            AddColumn("dbo.AjustesInventario", "Protegido", c => c.Boolean(nullable: false));
            AddColumn("dbo.AjustesInventario", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AjustesInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.AjustesInventario", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.AjustesInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.AjustesInventarioDetalle", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AjustesInventarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.AjustesInventarioDetalle", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.AjustesInventarioDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.DespachosInventario", "Protegido", c => c.Boolean(nullable: false));
            AddColumn("dbo.DespachosInventario", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DespachosInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.DespachosInventario", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.DespachosInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.DespachosInventarioDetalle", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DespachosInventarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.DespachosInventarioDetalle", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.DespachosInventarioDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.IngresosInventario", "Protegido", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolicitudDespacho", "Protegido", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolicitudDespacho", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SolicitudDespacho", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudDespacho", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.SolicitudDespacho", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudDespachoDetalle", "CantidadDespachada", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SolicitudDespachoDetalle", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SolicitudDespachoDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudDespachoDetalle", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.SolicitudDespachoDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudMaterialesSala", "Protegido", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolicitudMaterialesSalaDetalle", "CantidadDespachada", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.SolicitudDespacho", "HojaControlSala");
            DropColumn("dbo.SolicitudDespacho", "FechaOperacion");
            DropColumn("dbo.SolicitudDespacho", "Servicio");
            DropColumn("dbo.SolicitudDespacho", "Cama");
            DropColumn("dbo.SolicitudDespacho", "PacienteId");
            DropColumn("dbo.SolicitudDespacho", "Cirujano");
            DropColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaInstrumentista");
            DropColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaCirculante");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaCirculante", c => c.String(maxLength: 100));
            AddColumn("dbo.SolicitudDespacho", "AuxiliarEnfermeriaInstrumentista", c => c.String(maxLength: 100));
            AddColumn("dbo.SolicitudDespacho", "Cirujano", c => c.String(maxLength: 100));
            AddColumn("dbo.SolicitudDespacho", "PacienteId", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "Cama", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "Servicio", c => c.String(maxLength: 20));
            AddColumn("dbo.SolicitudDespacho", "FechaOperacion", c => c.DateTime());
            AddColumn("dbo.SolicitudDespacho", "HojaControlSala", c => c.String(maxLength: 10));
            DropColumn("dbo.SolicitudMaterialesSalaDetalle", "CantidadDespachada");
            DropColumn("dbo.SolicitudMaterialesSala", "Protegido");
            DropColumn("dbo.SolicitudDespachoDetalle", "LastModifiedBy");
            DropColumn("dbo.SolicitudDespachoDetalle", "LastModifiedDate");
            DropColumn("dbo.SolicitudDespachoDetalle", "CreatedBy");
            DropColumn("dbo.SolicitudDespachoDetalle", "CreatedDate");
            DropColumn("dbo.SolicitudDespachoDetalle", "CantidadDespachada");
            DropColumn("dbo.SolicitudDespacho", "LastModifiedBy");
            DropColumn("dbo.SolicitudDespacho", "LastModifiedDate");
            DropColumn("dbo.SolicitudDespacho", "CreatedBy");
            DropColumn("dbo.SolicitudDespacho", "CreatedDate");
            DropColumn("dbo.SolicitudDespacho", "Protegido");
            DropColumn("dbo.IngresosInventario", "Protegido");
            DropColumn("dbo.DespachosInventarioDetalle", "LastModifiedBy");
            DropColumn("dbo.DespachosInventarioDetalle", "LastModifiedDate");
            DropColumn("dbo.DespachosInventarioDetalle", "CreatedBy");
            DropColumn("dbo.DespachosInventarioDetalle", "CreatedDate");
            DropColumn("dbo.DespachosInventario", "LastModifiedBy");
            DropColumn("dbo.DespachosInventario", "LastModifiedDate");
            DropColumn("dbo.DespachosInventario", "CreatedBy");
            DropColumn("dbo.DespachosInventario", "CreatedDate");
            DropColumn("dbo.DespachosInventario", "Protegido");
            DropColumn("dbo.AjustesInventarioDetalle", "LastModifiedBy");
            DropColumn("dbo.AjustesInventarioDetalle", "LastModifiedDate");
            DropColumn("dbo.AjustesInventarioDetalle", "CreatedBy");
            DropColumn("dbo.AjustesInventarioDetalle", "CreatedDate");
            DropColumn("dbo.AjustesInventario", "LastModifiedBy");
            DropColumn("dbo.AjustesInventario", "LastModifiedDate");
            DropColumn("dbo.AjustesInventario", "CreatedBy");
            DropColumn("dbo.AjustesInventario", "CreatedDate");
            DropColumn("dbo.AjustesInventario", "Protegido");
            CreateIndex("dbo.SolicitudDespacho", "PacienteId");
            AddForeignKey("dbo.SolicitudDespacho", "PacienteId", "dbo.Pacientes", "PacienteId");
        }
    }
}
