namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DespachosInventarioDetalle", "ProveedorId", c => c.Int());
            AddColumn("dbo.DespachosInventarioDetalle", "Lote", c => c.String(maxLength: 20));
            AddColumn("dbo.DespachosInventarioDetalle", "FechaVencimiento", c => c.DateTime());
            AddColumn("dbo.DespachosInventarioDetalle", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.DespachosInventarioDetalle", "ProveedorId");
            AddForeignKey("dbo.DespachosInventarioDetalle", "ProveedorId", "dbo.Proveedores", "ProveedorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DespachosInventarioDetalle", "ProveedorId", "dbo.Proveedores");
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "ProveedorId" });
            DropColumn("dbo.DespachosInventarioDetalle", "Precio");
            DropColumn("dbo.DespachosInventarioDetalle", "FechaVencimiento");
            DropColumn("dbo.DespachosInventarioDetalle", "Lote");
            DropColumn("dbo.DespachosInventarioDetalle", "ProveedorId");
        }
    }
}
