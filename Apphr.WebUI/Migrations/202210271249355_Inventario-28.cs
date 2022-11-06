namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario28 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IngresosInventario", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.IngresosInventario", new[] { "BodegaId" });
            AddColumn("dbo.IngresosInvnetarioDetalle", "BodegaId", c => c.Int(nullable: false));
            CreateIndex("dbo.IngresosInvnetarioDetalle", "BodegaId");
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
            DropColumn("dbo.IngresosInventario", "BodegaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IngresosInventario", "BodegaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "BodegaId" });
            DropColumn("dbo.IngresosInvnetarioDetalle", "BodegaId");
            CreateIndex("dbo.IngresosInventario", "BodegaId");
            AddForeignKey("dbo.IngresosInventario", "BodegaId", "dbo.Bodegas", "BodegaId", cascadeDelete: false);
        }
    }
}
