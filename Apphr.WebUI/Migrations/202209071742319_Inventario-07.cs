namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SolicitudPedidoDT", "Valor", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            AlterColumn("dbo.SolicitudPedidoDT", "Precio", c => c.Decimal(precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SolicitudPedidoDT", "Precio", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.SolicitudPedidoDT", "Valor");
        }
    }
}
