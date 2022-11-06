namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario10 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IngresosInventarioDF", newName: "IngresosInventario");
            CreateTable(
                "dbo.IngresosInvnetarioDetalle",
                c => new
                    {
                        IngresoInventarioDetalleId = c.Long(nullable: false, identity: true),
                        IngresoInventarioId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IngresoInventarioDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .ForeignKey("dbo.IngresosInventario", t => t.IngresoInventarioId)
                .Index(t => t.IngresoInventarioId)
                .Index(t => t.MaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "IngresoInventarioId", "dbo.IngresosInventario");
            DropForeignKey("dbo.IngresosInvnetarioDetalle", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "MaterialId" });
            DropIndex("dbo.IngresosInvnetarioDetalle", new[] { "IngresoInventarioId" });
            DropTable("dbo.IngresosInvnetarioDetalle");
            RenameTable(name: "dbo.IngresosInventario", newName: "IngresosInventarioDF");
        }
    }
}
