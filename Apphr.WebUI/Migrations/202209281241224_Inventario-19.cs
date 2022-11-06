namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CierresInventario",
                c => new
                    {
                        CierreInventarioId = c.Long(nullable: false, identity: true),
                        BodegaId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CierreInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: false)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .Index(t => new { t.BodegaId, t.MaterialId, t.Fecha }, unique: true, name: "IX_MaterialFecha");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CierresInventario", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.CierresInventario", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.CierresInventario", "IX_MaterialFecha");
            DropTable("dbo.CierresInventario");
        }
    }
}
