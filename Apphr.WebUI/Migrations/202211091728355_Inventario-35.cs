namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenCompraDetalle", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.OrdenCompraDetalle", "OrdenCompraId");
            AddForeignKey("dbo.OrdenCompraDetalle", "OrdenCompraId", "dbo.OrdenCompra", "OrdenCompraId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdenCompraDetalle", "OrdenCompraId", "dbo.OrdenCompra");
            DropIndex("dbo.OrdenCompraDetalle", new[] { "OrdenCompraId" });
            DropColumn("dbo.OrdenCompraDetalle", "Precio");
        }
    }
}
