namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario13 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MovimientoInventarios", newName: "MovimientosInvnetario");
            RenameTable(name: "dbo.SolicitudDespachoDT", newName: "SolicitudDespachoDetalle");
            CreateTable(
                "dbo.ExistenciasBodega",
                c => new
                    {
                        ExistenciaBodegaId = c.Int(nullable: false, identity: true),
                        BodegaId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        ProveedorId = c.Int(),
                        Lote = c.String(maxLength: 20),
                        FechaVencimiento = c.DateTime(),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Minimo = c.Decimal(precision: 18, scale: 2),
                        Maximo = c.Decimal(precision: 18, scale: 2),
                        Pendiente = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ExistenciaBodegaId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId)
                .Index(t => t.BodegaId)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId);
            
            AddColumn("dbo.IngresosInvnetarioDetalle", "Observacion", c => c.String());
            AddColumn("dbo.MovimientosInvnetario", "Efecto", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.MovimientosInvnetario", "DocumentoReferencia", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExistenciasBodega", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.ExistenciasBodega", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.ExistenciasBodega", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.ExistenciasBodega", new[] { "ProveedorId" });
            DropIndex("dbo.ExistenciasBodega", new[] { "MaterialId" });
            DropIndex("dbo.ExistenciasBodega", new[] { "BodegaId" });
            DropColumn("dbo.MovimientosInvnetario", "DocumentoReferencia");
            DropColumn("dbo.MovimientosInvnetario", "Efecto");
            DropColumn("dbo.IngresosInvnetarioDetalle", "Observacion");
            DropTable("dbo.ExistenciasBodega");
            RenameTable(name: "dbo.SolicitudDespachoDetalle", newName: "SolicitudDespachoDT");
            RenameTable(name: "dbo.MovimientosInvnetario", newName: "MovimientoInventarios");
        }
    }
}
