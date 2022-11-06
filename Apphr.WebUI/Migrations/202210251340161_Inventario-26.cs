namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario26 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DespachosInventario", new[] { "BodegaId" });
            DropForeignKey("dbo.DespachosInventario", "BodegaId", "dbo.Bodegas");
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
                .PrimaryKey(t => t.ControlMaterialSalaId)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId, cascadeDelete: false)
                .Index(t => t.PacienteId);
            
            CreateTable(
                "dbo.ControlesMaterialSalaDetalle",
                c => new
                    {
                        ControlMaterialSalaDetalleId = c.Int(nullable: false, identity: true),
                        ControlMaterialSalaId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProveedorId = c.Int(nullable: false),
                        CasaComercial = c.String(),
                        Intercambio = c.Boolean(nullable: false),
                        linea = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControlMaterialSalaDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId, cascadeDelete: false)
                .ForeignKey("dbo.ControlesMaterialSala", t => t.ControlMaterialSalaId, cascadeDelete: false)
                .Index(t => t.ControlMaterialSalaId)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId);
            
            AddColumn("dbo.DespachosInventario", "TipoDocumentoReferencia", c => c.String(maxLength: 10));
            AddColumn("dbo.DespachosInventarioDetalle", "ControlMaterialSalaId", c => c.Int());
            AddColumn("dbo.DespachosInventarioDetalle", "BodegaId", c => c.Int(nullable: false));
            AlterColumn("dbo.DespachosInventario", "DocumentoReferencia", c => c.String(maxLength: 20));
            CreateIndex("dbo.DespachosInventarioDetalle", "BodegaId");
            AddForeignKey("dbo.DespachosInventarioDetalle", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
            //DropColumn("dbo.DespachosInventario", "BodegaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DespachosInventario", "BodegaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DespachosInventarioDetalle", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.ControlesMaterialSala", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ControlMaterialSalaId", "dbo.ControlesMaterialSala");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "BodegaId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ProveedorId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "MaterialId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ControlMaterialSalaId" });
            DropIndex("dbo.ControlesMaterialSala", new[] { "PacienteId" });
            AlterColumn("dbo.DespachosInventario", "DocumentoReferencia", c => c.String());
            DropColumn("dbo.DespachosInventarioDetalle", "BodegaId");
            DropColumn("dbo.DespachosInventarioDetalle", "ControlMaterialSalaId");
            DropColumn("dbo.DespachosInventario", "TipoDocumentoReferencia");
            DropTable("dbo.ControlesMaterialSalaDetalle");
            DropTable("dbo.ControlesMaterialSala");
            CreateIndex("dbo.DespachosInventario", "BodegaId");
            AddForeignKey("dbo.DespachosInventario", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
        }
    }
}
