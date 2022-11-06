namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TiposMovimientoInventario",
                c => new
                    {
                        TipoMovimientoInventarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 25),
                        NombreCorto = c.String(maxLength: 8),
                        Efecto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TipoMovimientoInventarioId);
            
            AddColumn("dbo.MovimientosInvnetario", "Documento", c => c.String(maxLength: 25));
            AddColumn("dbo.MovimientosInvnetario", "TipoMovimientoInventarioId", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientosInvnetario", "DestinoId", c => c.Int());
            AlterColumn("dbo.MovimientosInvnetario", "DocumentoReferencia", c => c.String(maxLength: 25));
            CreateIndex("dbo.MovimientosInvnetario", "TipoMovimientoInventarioId");
            CreateIndex("dbo.MovimientosInvnetario", "DestinoId");
            AddForeignKey("dbo.MovimientosInvnetario", "DestinoId", "dbo.Destinos", "DestinoId");
            AddForeignKey("dbo.MovimientosInvnetario", "TipoMovimientoInventarioId", "dbo.TiposMovimientoInventario", "TipoMovimientoInventarioId", cascadeDelete: true);
            DropColumn("dbo.MovimientosInvnetario", "TipoDocumento");
            Sql(@"SET IDENTITY_INSERT TiposMovimientoInventario ON
                INSERT INTO TiposMovimientoInventario (TipoMovimientoInventarioId, Nombre, NombreCorto, Efecto)
                VALUES
                (1, 'Ingreso Inventario',  'Ing.Inv.',  1),
                (2, 'Despacho Inventario', 'Des.Inv.', -1),
                (3, 'Ingreso Ajuste',      'Ingr.Aj.',  1),
                (4, 'Egreso Ajuste',       'Egre.Aj.', -1);
                SET IDENTITY_INSERT TiposMovimientoInventario OFF");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovimientosInvnetario", "TipoDocumento", c => c.String(maxLength: 25));
            DropForeignKey("dbo.MovimientosInvnetario", "TipoMovimientoInventarioId", "dbo.TiposMovimientoInventario");
            DropForeignKey("dbo.MovimientosInvnetario", "DestinoId", "dbo.Destinos");
            DropIndex("dbo.MovimientosInvnetario", new[] { "DestinoId" });
            DropIndex("dbo.MovimientosInvnetario", new[] { "TipoMovimientoInventarioId" });
            AlterColumn("dbo.MovimientosInvnetario", "DocumentoReferencia", c => c.String());
            DropColumn("dbo.MovimientosInvnetario", "DestinoId");
            DropColumn("dbo.MovimientosInvnetario", "TipoMovimientoInventarioId");
            DropColumn("dbo.MovimientosInvnetario", "Documento");
            DropTable("dbo.TiposMovimientoInventario");
        }
    }
}
