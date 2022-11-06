namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AjustesInventario",
                c => new
                    {
                        AjusteInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoReferencia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        Lineas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AjusteInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: false)
                .Index(t => t.BodegaId);
            
            CreateTable(
                "dbo.AjustesInventarioDetalle",
                c => new
                    {
                        AjusteInventarioDetalleId = c.Long(nullable: false, identity: true),
                        AjusteInventarioId = c.String(maxLength: 15),
                        Linea = c.Int(nullable: false),
                        TipoMovimientoId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        ProveedorId = c.Int(),
                        Lote = c.String(maxLength: 20),
                        FechaVencimiento = c.DateTime(),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observacion = c.String(),
                        MovimientoInventarioId = c.Long(nullable: false),
                        TipoMovmientoInventario_TipoMovimientoInventarioId = c.Int(),
                    })
                .PrimaryKey(t => t.AjusteInventarioDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .ForeignKey("dbo.MovimientosInvnetario", t => t.MovimientoInventarioId, cascadeDelete: false)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId)
                .ForeignKey("dbo.TiposMovimientoInventario", t => t.TipoMovmientoInventario_TipoMovimientoInventarioId)
                .ForeignKey("dbo.AjustesInventario", t => t.AjusteInventarioId)
                .Index(t => t.AjusteInventarioId)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId)
                .Index(t => t.MovimientoInventarioId)
                .Index(t => t.TipoMovmientoInventario_TipoMovimientoInventarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AjustesInventarioDetalle", "AjusteInventarioId", "dbo.AjustesInventario");
            DropForeignKey("dbo.AjustesInventarioDetalle", "TipoMovmientoInventario_TipoMovimientoInventarioId", "dbo.TiposMovimientoInventario");
            DropForeignKey("dbo.AjustesInventarioDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.AjustesInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropForeignKey("dbo.AjustesInventarioDetalle", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.AjustesInventario", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "TipoMovmientoInventario_TipoMovimientoInventarioId" });
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "MovimientoInventarioId" });
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "ProveedorId" });
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "MaterialId" });
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "AjusteInventarioId" });
            DropIndex("dbo.AjustesInventario", new[] { "BodegaId" });
            DropTable("dbo.AjustesInventarioDetalle");
            DropTable("dbo.AjustesInventario");
        }
    }
}
