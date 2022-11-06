namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario30 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ControlMaterialSalaId", "dbo.ControlesMaterialSala");
            DropForeignKey("dbo.ControlesMaterialSala", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.ControlesMaterialSala", new[] { "PacienteId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ControlMaterialSalaId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "MaterialId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ProveedorId" });
            CreateTable(
                "dbo.SolicitudMaterialesSala",
                c => new
                    {
                        SolicitudMaterialSalaId = c.String(nullable: false, maxLength: 15),
                        FechaOperacion = c.DateTime(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        HojaControlSala = c.String(),
                        Cama = c.Int(nullable: false),
                        PacienteId = c.Int(nullable: false),
                        Servicio = c.String(maxLength: 20),
                        Cirujano = c.String(maxLength: 100),
                        AuxiliarEnfermeriaInstrumentista = c.String(maxLength: 100),
                        AuxiliarEnfermeriaCirculante = c.String(maxLength: 100),
                        Observacion = c.String(),
                        DespachoInventarioId = c.String(maxLength: 15),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SolicitudMaterialSalaId)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId, cascadeDelete: false)
                .Index(t => t.PacienteId);
            
            CreateTable(
                "dbo.SolicitudMaterialesSalaDetalle",
                c => new
                    {
                        SolicitudMaterialSalaDetalleId = c.Int(nullable: false, identity: true),
                        SolicitudMaterialSalaId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProveedorId = c.Int(),
                        CasaComercial = c.String(),
                        Intercambio = c.Boolean(nullable: false),
                        linea = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SolicitudMaterialSalaDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId)
                .ForeignKey("dbo.SolicitudMaterialesSala", t => t.SolicitudMaterialSalaId)
                .Index(t => t.SolicitudMaterialSalaId)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId);
            
            DropTable("dbo.ControlesMaterialSala");
            DropTable("dbo.ControlesMaterialSalaDetalle");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ControlesMaterialSalaDetalle",
                c => new
                    {
                        ControlMaterialSalaDetalleId = c.Int(nullable: false, identity: true),
                        ControlMaterialSalaId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProveedorId = c.Int(),
                        CasaComercial = c.String(),
                        Intercambio = c.Boolean(nullable: false),
                        linea = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControlMaterialSalaDetalleId);
            
            CreateTable(
                "dbo.ControlesMaterialSala",
                c => new
                    {
                        ControlMaterialSalaId = c.Int(nullable: false, identity: true),
                        FechaOperacion = c.DateTime(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        HojaControlSala = c.String(),
                        Cama = c.Int(nullable: false),
                        PacienteId = c.Int(nullable: false),
                        Servicio = c.String(maxLength: 20),
                        Cirujano = c.String(maxLength: 100),
                        AuxiliarEnfermeriaInstrumentista = c.String(maxLength: 100),
                        AuxiliarEnfermeriaCirculante = c.String(maxLength: 100),
                        Observacion = c.String(),
                        DespachoInventarioId = c.String(maxLength: 15),
                        Lineas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControlMaterialSalaId);
            
            DropForeignKey("dbo.SolicitudMaterialesSala", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.SolicitudMaterialesSalaDetalle", "SolicitudMaterialSalaId", "dbo.SolicitudMaterialesSala");
            DropForeignKey("dbo.SolicitudMaterialesSalaDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.SolicitudMaterialesSalaDetalle", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.SolicitudMaterialesSalaDetalle", new[] { "ProveedorId" });
            DropIndex("dbo.SolicitudMaterialesSalaDetalle", new[] { "MaterialId" });
            DropIndex("dbo.SolicitudMaterialesSalaDetalle", new[] { "SolicitudMaterialSalaId" });
            DropIndex("dbo.SolicitudMaterialesSala", new[] { "PacienteId" });
            DropTable("dbo.SolicitudMaterialesSalaDetalle");
            DropTable("dbo.SolicitudMaterialesSala");
            CreateIndex("dbo.ControlesMaterialSalaDetalle", "ProveedorId");
            CreateIndex("dbo.ControlesMaterialSalaDetalle", "MaterialId");
            CreateIndex("dbo.ControlesMaterialSalaDetalle", "ControlMaterialSalaId");
            CreateIndex("dbo.ControlesMaterialSala", "PacienteId");
            AddForeignKey("dbo.ControlesMaterialSala", "PacienteId", "dbo.Pacientes", "PacienteId", cascadeDelete: false);
            AddForeignKey("dbo.ControlesMaterialSalaDetalle", "ControlMaterialSalaId", "dbo.ControlesMaterialSala", "ControlMaterialSalaId", cascadeDelete: false);
            AddForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores", "ProveedorId");
            AddForeignKey("dbo.ControlesMaterialSalaDetalle", "MaterialId", "dbo.Materiales", "MaterialId", cascadeDelete: false);
        }
    }
}
