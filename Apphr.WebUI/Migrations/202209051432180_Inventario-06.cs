namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SolicitudPedidoDF", "SeccionId", c => c.Int(nullable: false));
            CreateIndex("dbo.SolicitudPedidoDF", "SeccionId");
            AddForeignKey("dbo.SolicitudPedidoDF", "SeccionId", "dbo.Destinos", "DestinoId", cascadeDelete: false);
            DropColumn("dbo.SolicitudPedidoDF", "DepartamentoSeccionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudPedidoDF", "DepartamentoSeccionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SolicitudPedidoDF", "SeccionId", "dbo.Destinos");
            DropIndex("dbo.SolicitudPedidoDF", new[] { "SeccionId" });
            DropColumn("dbo.SolicitudPedidoDF", "SeccionId");
        }
    }
}
