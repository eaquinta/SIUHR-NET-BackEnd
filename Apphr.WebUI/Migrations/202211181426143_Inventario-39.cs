namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EgresosAbastecimiento",
                c => new
                    {
                        EgresoAbastecimientoId = c.Int(nullable: false, identity: true),
                        SolicitudMaterialSalaId = c.String(maxLength: 15),
                        SolicitudMaterialSalaDetalleId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        OrdenCompraId = c.String(maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.EgresoAbastecimientoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EgresosAbastecimiento");
        }
    }
}
