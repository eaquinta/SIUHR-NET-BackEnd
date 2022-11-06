namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EgresosInventario",
                c => new
                    {
                        EgresoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoRefenrecia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.EgresoInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true)
                .Index(t => t.BodegaId);
            
            CreateTable(
                "dbo.EgresosInvnetarioDetalle",
                c => new
                    {
                        EgresoInventarioDetalleId = c.Long(nullable: false, identity: true),
                        Linea = c.Int(nullable: false),
                        EgresoInventarioId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        Lote = c.String(maxLength: 20),
                        FechaVencimiento = c.DateTime(),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observacion = c.String(),
                        MovimientoInventarioId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.EgresoInventarioDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.MovimientosInvnetario", t => t.MovimientoInventarioId, cascadeDelete: false)
                .ForeignKey("dbo.EgresosInventario", t => t.EgresoInventarioId)
                .Index(t => t.EgresoInventarioId)
                .Index(t => t.MaterialId)
                .Index(t => t.MovimientoInventarioId);
            
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        PacienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PacienteId)
                .ForeignKey("dbo.Personas", t => t.PacienteId)
                .Index(t => t.PacienteId);
            
            CreateTable(
                "dbo.Medicos",
                c => new
                    {
                        MedicoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicoId)
                .ForeignKey("dbo.Personas", t => t.MedicoId)
                .Index(t => t.MedicoId);
            
            AddColumn("dbo.IngresosInventario", "Lineas", c => c.Int(nullable: false));
            AddColumn("dbo.IngresosInvnetarioDetalle", "Linea", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientosInvnetario", "TipoDocumento", c => c.String(maxLength: 25));
            AddColumn("dbo.MovimientosInvnetario", "Line", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientosInvnetario", "PacienteId", c => c.Int());
            CreateIndex("dbo.MovimientosInvnetario", "PacienteId");
            AddForeignKey("dbo.MovimientosInvnetario", "PacienteId", "dbo.Pacientes", "PacienteId", cascadeDelete:false);
            DropColumn("dbo.MovimientosInvnetario", "Tipo");
            DropColumn("dbo.MovimientosInvnetario", "Correlativo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovimientosInvnetario", "Correlativo", c => c.String());
            AddColumn("dbo.MovimientosInvnetario", "Tipo", c => c.String(maxLength: 3));
            DropForeignKey("dbo.Medicos", "MedicoId", "dbo.Personas");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "EgresoInventarioId", "dbo.EgresosInventario");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropForeignKey("dbo.MovimientosInvnetario", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.Pacientes", "PacienteId", "dbo.Personas");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.EgresosInventario", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.Medicos", new[] { "MedicoId" });
            DropIndex("dbo.Pacientes", new[] { "PacienteId" });
            DropIndex("dbo.MovimientosInvnetario", new[] { "PacienteId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "MovimientoInventarioId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "MaterialId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "EgresoInventarioId" });
            DropIndex("dbo.EgresosInventario", new[] { "BodegaId" });
            DropColumn("dbo.MovimientosInvnetario", "PacienteId");
            DropColumn("dbo.MovimientosInvnetario", "Line");
            DropColumn("dbo.MovimientosInvnetario", "TipoDocumento");
            DropColumn("dbo.IngresosInvnetarioDetalle", "Linea");
            DropColumn("dbo.IngresosInventario", "Lineas");
            DropTable("dbo.Medicos");
            DropTable("dbo.Pacientes");
            DropTable("dbo.EgresosInvnetarioDetalle");
            DropTable("dbo.EgresosInventario");
        }
    }
}
