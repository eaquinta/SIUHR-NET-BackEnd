namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario16 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DespachosInventarioDF", newName: "DespachosInventario");
            RenameTable(name: "dbo.DespachosInventarioDT", newName: "DespachosInventarioDetalle");
            RenameColumn(table: "dbo.DespachosInventarioDetalle", name: "DespachoInventarioDF_DespachoInventarioId", newName: "DespachoInventarioId");
            RenameIndex(table: "dbo.DespachosInventarioDetalle", name: "IX_DespachoInventarioDF_DespachoInventarioId", newName: "IX_DespachoInventarioId");
            AddColumn("dbo.DespachosInventario", "DocumentoRefenrecia", c => c.String());
            AddColumn("dbo.DespachosInventario", "FechaDocumentoReferencia", c => c.DateTime(nullable: false));
            AddColumn("dbo.DespachosInventario", "Lineas", c => c.Int(nullable: false));
            AddColumn("dbo.DespachosInventarioDetalle", "Linea", c => c.Int(nullable: false));
            AddColumn("dbo.DespachosInventarioDetalle", "Observacion", c => c.String());
            AddColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", c => c.Guid(nullable: false));
            CreateIndex("dbo.DespachosInventarioDetalle", "MovimientoInventarioId");
            AddForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario", "MovimientoInventarioId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DespachosInventarioDetalle", "MovimientoInventarioId", "dbo.MovimientosInvnetario");
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "MovimientoInventarioId" });
            DropColumn("dbo.DespachosInventarioDetalle", "MovimientoInventarioId");
            DropColumn("dbo.DespachosInventarioDetalle", "Observacion");
            DropColumn("dbo.DespachosInventarioDetalle", "Linea");
            DropColumn("dbo.DespachosInventario", "Lineas");
            DropColumn("dbo.DespachosInventario", "FechaDocumentoReferencia");
            DropColumn("dbo.DespachosInventario", "DocumentoRefenrecia");
            RenameIndex(table: "dbo.DespachosInventarioDetalle", name: "IX_DespachoInventarioId", newName: "IX_DespachoInventarioDF_DespachoInventarioId");
            RenameColumn(table: "dbo.DespachosInventarioDetalle", name: "DespachoInventarioId", newName: "DespachoInventarioDF_DespachoInventarioId");
            RenameTable(name: "dbo.DespachosInventarioDetalle", newName: "DespachosInventarioDT");
            RenameTable(name: "dbo.DespachosInventario", newName: "DespachosInventarioDF");
        }
    }
}
