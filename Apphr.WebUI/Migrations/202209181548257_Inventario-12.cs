namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", c => c.Guid(nullable: false));
            AddColumn("dbo.MovimientoInventarios", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId");
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientoInventarios", "MovimientoInventarioId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientoInventarios");
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "MovimientoInventarioId" });
            DropColumn("dbo.MovimientoInventarios", "Precio");
            DropColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId");
        }
    }
}
