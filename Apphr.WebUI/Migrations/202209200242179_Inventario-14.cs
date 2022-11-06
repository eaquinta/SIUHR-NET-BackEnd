namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IngresosInvnetarioDetalle", "Lote", c => c.String(maxLength: 20));
            AddColumn("dbo.MovimientosInvnetario", "Observacion", c => c.String());
            AlterColumn("dbo.MovimientosInvnetario", "Lote", c => c.String(maxLength: 20));
            DropColumn("dbo.MovimientosInvnetario", "Observaciones");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovimientosInvnetario", "Observaciones", c => c.String());
            AlterColumn("dbo.MovimientosInvnetario", "Lote", c => c.String());
            DropColumn("dbo.MovimientosInvnetario", "Observacion");
            DropColumn("dbo.IngresosInvnetarioDetalle", "Lote");
        }
    }
}
