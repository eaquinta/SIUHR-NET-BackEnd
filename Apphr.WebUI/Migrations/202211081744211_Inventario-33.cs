namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdenCompra",
                c => new
                    {
                        OrdenCompraId = c.String(nullable: false, maxLength: 15),
                        ProveedorId = c.Int(nullable: false),
                        Autorizacion = c.String(maxLength: 10),
                        Fecha = c.DateTime(nullable: false),
                        FechaAutorizacion = c.DateTime(),
                        ValorFacturado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaFacturado = c.DateTime(),
                        Status = c.String(maxLength: 10),
                        SolicitudPedidoId = c.String(maxLength: 15),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.OrdenCompraId)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId, cascadeDelete: false)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.OrdenCompraDetalle",
                c => new
                    {
                        OrdenCompraDetalleId = c.Int(nullable: false, identity: true),
                        OrdenCompraId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CantidadRecibido = c.Decimal(precision: 18, scale: 2),
                        ValorRecibido = c.Decimal(precision: 18, scale: 2),
                        Observacion = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.OrdenCompraDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .Index(t => t.MaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdenCompraDetalle", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.OrdenCompra", "ProveedorId", "dbo.Proveedores");
            DropIndex("dbo.OrdenCompraDetalle", new[] { "MaterialId" });
            DropIndex("dbo.OrdenCompra", new[] { "ProveedorId" });
            DropTable("dbo.OrdenCompraDetalle");
            DropTable("dbo.OrdenCompra");
        }
    }
}
