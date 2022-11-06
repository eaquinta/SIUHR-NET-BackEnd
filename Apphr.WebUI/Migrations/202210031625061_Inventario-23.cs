namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario23 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MovimientosInvnetario");
            DropColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId");
            AddColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId", c => c.Long(nullable: false, identity: true));
            //AlterColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.MovimientosInvnetario", "MovimientoInventarioId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MovimientosInvnetario");
            DropColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId");
            AddColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId", c => c.Guid(nullable: false));
            //AlterColumn("dbo.MovimientosInvnetario", "MovimientoInventarioId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.MovimientosInvnetario", "MovimientoInventarioId");
        }
    }
}
