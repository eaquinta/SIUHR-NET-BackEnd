namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario32 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudPedido", "SeccionId", "dbo.Destinos");
            DropIndex("dbo.SolicitudPedido", new[] { "SeccionId" });
            DropPrimaryKey("dbo.SolicitudPedidoDetalle");
            DropColumn("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDTId");
            AddColumn("dbo.SolicitudPedido", "Protegida", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolicitudPedido", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SolicitudPedido", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudPedido", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.SolicitudPedido", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDetalleId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SolicitudPedidoDetalle", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SolicitudPedidoDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudPedidoDetalle", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.SolicitudPedidoDetalle", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.SolicitudPedido", "SeccionId", c => c.Int());
            AddPrimaryKey("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDetalleId");
            CreateIndex("dbo.SolicitudPedido", "SeccionId");
            AddForeignKey("dbo.SolicitudPedido", "SeccionId", "dbo.Destinos", "DestinoId");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDTId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.SolicitudPedido", "SeccionId", "dbo.Destinos");
            DropIndex("dbo.SolicitudPedido", new[] { "SeccionId" });
            DropPrimaryKey("dbo.SolicitudPedidoDetalle");
            AlterColumn("dbo.SolicitudPedido", "SeccionId", c => c.Int(nullable: false));
            DropColumn("dbo.SolicitudPedidoDetalle", "LastModifiedBy");
            DropColumn("dbo.SolicitudPedidoDetalle", "LastModifiedDate");
            DropColumn("dbo.SolicitudPedidoDetalle", "CreatedBy");
            DropColumn("dbo.SolicitudPedidoDetalle", "CreatedDate");
            DropColumn("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDetalleId");
            DropColumn("dbo.SolicitudPedido", "LastModifiedBy");
            DropColumn("dbo.SolicitudPedido", "LastModifiedDate");
            DropColumn("dbo.SolicitudPedido", "CreatedBy");
            DropColumn("dbo.SolicitudPedido", "CreatedDate");
            DropColumn("dbo.SolicitudPedido", "Protegida");
            AddPrimaryKey("dbo.SolicitudPedidoDetalle", "SolicitudPedidoDTId");
            CreateIndex("dbo.SolicitudPedido", "SeccionId");
            AddForeignKey("dbo.SolicitudPedido", "SeccionId", "dbo.Destinos", "DestinoId", cascadeDelete: false);
        }
    }
}
