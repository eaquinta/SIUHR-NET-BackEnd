namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario29 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AjustesInventario", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores");
            DropIndex("dbo.AjustesInventario", new[] { "BodegaId" });
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ProveedorId" });
            AddColumn("dbo.AjustesInventarioDetalle", "BodegaId", c => c.Int(nullable: false));
            AlterColumn("dbo.ControlesMaterialSalaDetalle", "ProveedorId", c => c.Int());
            AlterColumn("dbo.SolicitudDespachoDetalle", "Cantidad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.AjustesInventarioDetalle", "BodegaId");
            CreateIndex("dbo.ControlesMaterialSalaDetalle", "ProveedorId");
            AddForeignKey("dbo.AjustesInventarioDetalle", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
            AddForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores", "ProveedorId");
            DropColumn("dbo.AjustesInventario", "BodegaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AjustesInventario", "BodegaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.AjustesInventarioDetalle", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.ControlesMaterialSalaDetalle", new[] { "ProveedorId" });
            DropIndex("dbo.AjustesInventarioDetalle", new[] { "BodegaId" });
            AlterColumn("dbo.SolicitudDespachoDetalle", "Cantidad", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ControlesMaterialSalaDetalle", "ProveedorId", c => c.Int(nullable: false));
            DropColumn("dbo.AjustesInventarioDetalle", "BodegaId");
            CreateIndex("dbo.ControlesMaterialSalaDetalle", "ProveedorId");
            CreateIndex("dbo.AjustesInventario", "BodegaId");
            AddForeignKey("dbo.ControlesMaterialSalaDetalle", "ProveedorId", "dbo.Proveedores", "ProveedorId", cascadeDelete: false);
            AddForeignKey("dbo.AjustesInventario", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
        }
    }
}
