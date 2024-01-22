namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SolicitudPedido", "TipoEvento", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SolicitudPedido", "TipoEvento");
        }
    }
}
