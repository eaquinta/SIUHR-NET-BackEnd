namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlAbastecimiento",
                c => new
                    {
                        DestinoId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DestinoId, t.MaterialId });
            
            AddColumn("dbo.SolicitudPedidoDetalle", "OrdenCompraId", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SolicitudPedidoDetalle", "OrdenCompraId");
            DropTable("dbo.ControlAbastecimiento");
        }
    }
}
