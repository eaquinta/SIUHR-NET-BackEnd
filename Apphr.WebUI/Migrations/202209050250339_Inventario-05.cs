namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario05 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SolicitudPedidoDF", "DepartamentoId");
            AddForeignKey("dbo.SolicitudPedidoDF", "DepartamentoId", "dbo.Destinos", "DestinoId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SolicitudPedidoDF", "DepartamentoId", "dbo.Destinos");
            DropIndex("dbo.SolicitudPedidoDF", new[] { "DepartamentoId" });
        }
    }
}
