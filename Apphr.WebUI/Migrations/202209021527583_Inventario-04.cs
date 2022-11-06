namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario04 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudPedidoDT", "SolicitudId", "dbo.SolicitudPedidoDF");
            DropPrimaryKey("dbo.SolicitudPedidoDF");
            AddColumn("dbo.SolicitudPedidoDF", "SolicitudPedidoId", c => c.String(nullable: false, maxLength: 15));
            Sql("UPDATE dbo.SolicitudPedidoDF SET SolicitudPedidoId = SolicitudId");
            AddPrimaryKey("dbo.SolicitudPedidoDF", "SolicitudPedidoId");
            AddForeignKey("dbo.SolicitudPedidoDT", "SolicitudId", "dbo.SolicitudPedidoDF", "SolicitudPedidoId");
            DropColumn("dbo.SolicitudPedidoDF", "SolicitudId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudPedidoDF", "SolicitudId", c => c.String(nullable: false, maxLength: 15));
            Sql("UPDATE dbo.SolicitudPedidoDF SET SolicitudId = SolicitudPedidoId");
            DropForeignKey("dbo.SolicitudPedidoDT", "SolicitudId", "dbo.SolicitudPedidoDF");
            DropPrimaryKey("dbo.SolicitudPedidoDF");
            DropColumn("dbo.SolicitudPedidoDF", "SolicitudPedidoId");
            AddPrimaryKey("dbo.SolicitudPedidoDF", "SolicitudId");
            AddForeignKey("dbo.SolicitudPedidoDT", "SolicitudId", "dbo.SolicitudPedidoDF", "SolicitudId");
        }
    }
}
