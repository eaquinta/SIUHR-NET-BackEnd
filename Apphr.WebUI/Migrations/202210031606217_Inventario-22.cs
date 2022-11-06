namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropForeignKey("dbo.EgresosInventario", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropForeignKey("dbo.EgresosInvnetarioDetalle", "EgresoInventarioId", "dbo.EgresosInventario");
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "MovimientoInventarioId" });
            DropIndex("dbo.EgresosInventario", new[] { "BodegaId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "EgresoInventarioId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "MaterialId" });
            DropIndex("dbo.EgresosInvnetarioDetalle", new[] { "MovimientoInventarioId" });
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "MovimientoInventarioId" });
            DropTable("dbo.EgresosInventario");
            DropTable("dbo.EgresosInvnetarioDetalle");
            Sql(@"ALTER TABLE [dbo].[IngresosInvnetarioDetalle] DROP CONSTRAINT [FK_dbo.IngresosInvnetarioDetalle_dbo.MovimientoInventarios_MovimientoInventarioId]");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.EgresoInventarioDetalleId);
            
            CreateTable(
                "dbo.EgresosInventario",
                c => new
                    {
                        EgresoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoReferencia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.EgresoInventarioId);
            
            CreateIndex("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId");
            CreateIndex("dbo.EgresosInvnetarioDetalle", "MovimientoInventarioId");
            CreateIndex("dbo.EgresosInvnetarioDetalle", "MaterialId");
            CreateIndex("dbo.EgresosInvnetarioDetalle", "EgresoInventarioId");
            CreateIndex("dbo.EgresosInventario", "BodegaId");
            CreateIndex("dbo.DespachosInventarioDetalle", "MovimientoInventarioId");
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
            AddForeignKey("dbo.EgresosInvnetarioDetalle", "EgresoInventarioId", "dbo.EgresosInventario", "EgresoInventarioId");
            AddForeignKey("dbo.EgresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
            AddForeignKey("dbo.EgresosInvnetarioDetalle", "MaterialId", "dbo.Materiales", "MaterialId", cascadeDelete: false);
            AddForeignKey("dbo.EgresosInventario", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
            AddForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
        }
    }
}
