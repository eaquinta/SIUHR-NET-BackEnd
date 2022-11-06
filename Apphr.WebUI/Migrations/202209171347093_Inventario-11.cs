namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario11 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SolicitudPedidoDF", newName: "SolicitudPedido");
            RenameTable(name: "dbo.SolicitudPedidoDT", newName: "SolicitudPedidoDetalle");
            DropForeignKey("dbo.IngresosInventario", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "IngresoInventarioId", "dbo.IngresosInventario");
            DropTable("dbo.IngresosInventario");
            CreateTable(
                "dbo.IngresosInventario",
                c => new
                    {
                        IngresoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoRefenrecia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.IngresoInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true);
            
            AddColumn("dbo.IngresosInvnetarioDetalle", "ProveedorId", c => c.Int());
            AddColumn("dbo.IngresosInvnetarioDetalle", "FechaVencimiento", c => c.DateTime());
            AddColumn("dbo.IngresosInvnetarioDetalle", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.IngresosInvnetarioDetalle", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.IngresosInvnetarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedBy", c => c.String());
            CreateIndex("dbo.IngresosInvnetarioDetalle", "ProveedorId");
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "ProveedorId", "dbo.Proveedores", "ProveedorId");
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "IngresoInventarioId", "dbo.IngresosInventario", "IngresoInventarioId");            
        }
        
        public override void Down()
        {
            DropTable("dbo.IngresosInventario");
            CreateTable(
                "dbo.IngresosInventario",
                c => new
                    {
                        IngresoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoRefenrecia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IngresoInventarioId);
            
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "IngresoInventarioId", "dbo.IngresosInventario");
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.IngresosInventario", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "ProveedorId" });
            DropColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedBy");
            DropColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedDate");
            DropColumn("dbo.IngresosInvnetarioDetalle", "CreatedBy");
            DropColumn("dbo.IngresosInvnetarioDetalle", "CreatedDate");
            DropColumn("dbo.IngresosInvnetarioDetalle", "Precio");
            DropColumn("dbo.IngresosInvnetarioDetalle", "FechaVencimiento");
            DropColumn("dbo.IngresosInvnetarioDetalle", "ProveedorId");            
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "IngresoInventarioId", "dbo.IngresosInventario", "IngresoInventarioId");
            AddForeignKey("dbo.IngresosInventario", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: true);
            RenameTable(name: "dbo.SolicitudPedidoDetalle", newName: "SolicitudPedidoDT");
            RenameTable(name: "dbo.SolicitudPedido", newName: "SolicitudPedidoDF");
        }
    }
}
