namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario42 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InicialAbastecimiento",
                c => new
                    {
                        MaterialId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.MaterialId, t.ProveedorId })
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId, cascadeDelete: false)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId);
            
            AddColumn("dbo.MovimientosAbastecimiento", "ProveedorId", c => c.Int());
            CreateIndex("dbo.MovimientosAbastecimiento", "ProveedorId");
            AddForeignKey("dbo.MovimientosAbastecimiento", "ProveedorId", "dbo.Proveedores", "ProveedorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimientosAbastecimiento", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.InicialAbastecimiento", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.InicialAbastecimiento", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.MovimientosAbastecimiento", new[] { "ProveedorId" });
            DropIndex("dbo.InicialAbastecimiento", new[] { "ProveedorId" });
            DropIndex("dbo.InicialAbastecimiento", new[] { "MaterialId" });
            DropColumn("dbo.MovimientosAbastecimiento", "ProveedorId");
            DropTable("dbo.InicialAbastecimiento");
        }
    }
}
