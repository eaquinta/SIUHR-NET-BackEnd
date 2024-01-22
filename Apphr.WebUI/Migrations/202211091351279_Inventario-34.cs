namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario34 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SolicitudPedidoDetalle", name: "SolicitudId", newName: "SolicitudPedidoId");
            RenameIndex(table: "dbo.SolicitudPedidoDetalle", name: "IX_SolicitudId", newName: "IX_SolicitudPedidoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SolicitudPedidoDetalle", name: "IX_SolicitudPedidoId", newName: "IX_SolicitudId");
            RenameColumn(table: "dbo.SolicitudPedidoDetalle", name: "SolicitudPedidoId", newName: "SolicitudId");
        }
    }
}
