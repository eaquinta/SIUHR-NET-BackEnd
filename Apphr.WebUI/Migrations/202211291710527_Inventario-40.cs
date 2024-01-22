namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario40 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovimientosAbastecimiento",
                c => new
                    {
                        MovimientoAbastecimientoId = c.Int(nullable: false, identity: true),
                        TipoMovimiento = c.String(maxLength: 3),
                        Efecto = c.Int(nullable: false),
                        DestinoId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        SolicitudPedidoId = c.String(maxLength: 15),
                        OrdenCompraId = c.String(maxLength: 15),
                        FechaIngreso = c.DateTime(),
                        SolicitudMaterialSalaId = c.String(maxLength: 15),
                        SolicitudMaterialSalaDetalleId = c.Int(),
                        FechaEgreso = c.DateTime(),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.MovimientoAbastecimientoId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: false)
                .Index(t => t.MaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimientosAbastecimiento", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.MovimientosAbastecimiento", new[] { "MaterialId" });
            DropTable("dbo.MovimientosAbastecimiento");
        }
    }
}
