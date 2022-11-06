namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario24 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId");
            AddColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", c => c.Long(nullable: false));
            //AlterColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", c => c.Long(nullable: false));
            DropColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId");
            AddColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", c => c.Long(nullable: false));
            //AlterColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", c => c.Long(nullable: false));
            CreateIndex("dbo.DespachosInventarioDetalle", "MovimientoInventarioId");
            CreateIndex("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId");
            AddForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
            AddForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "MovimientoInventarioId" });
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "MovimientoInventarioId" });
            AlterColumn("dbo.IngresosInvnetarioDetalle", "MovimientoInventarioId", c => c.Guid(nullable: false));
            AlterColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", c => c.Guid(nullable: false));
        }
    }
}
