namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario38 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IngresosAbastecimiento",
                c => new
                    {
                        IngresoAbastecimientoId = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        OrdenCompraId = c.String(maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IngresoAbastecimientoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IngresosAbastecimiento");
        }
    }
}
