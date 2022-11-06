namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Proveedor", "PersonId", "dbo.Personas");
            DropIndex("dbo.Proveedor", new[] { "Nit" });
            DropIndex("dbo.Proveedor", new[] { "PersonId" });
            CreateTable(
                "dbo.DespachosInventarioDF",
                c => new
                    {
                        DespachoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DestinoId = c.Int(nullable: false),
                        BodegaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DespachoInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true)
                .ForeignKey("dbo.Destinos", t => t.DestinoId, cascadeDelete: true)
                .Index(t => t.DestinoId)
                .Index(t => t.BodegaId);
            
            CreateTable(
                "dbo.DespachosInventarioDT",
                c => new
                    {
                        DespachoInventarioDetalleId = c.Long(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DespachoInventarioDF_DespachoInventarioId = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.DespachoInventarioDetalleId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.DespachosInventarioDF", t => t.DespachoInventarioDF_DespachoInventarioId)
                .Index(t => t.MaterialId)
                .Index(t => t.DespachoInventarioDF_DespachoInventarioId);
            
            CreateTable(
                "dbo.IngresosInventarioDF",
                c => new
                    {
                        IngresoInventarioId = c.String(nullable: false, maxLength: 15),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoRefenrecia = c.String(),
                        FechaDocumentoReferencia = c.DateTime(nullable: false),
                        BodegaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IngresoInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true)
                .Index(t => t.BodegaId);
            
            CreateTable(
                "dbo.MovimientoInventarios",
                c => new
                    {
                        MovimientoInventarioId = c.Guid(nullable: false),
                        Tipo = c.String(maxLength: 3),
                        Correlativo = c.String(),
                        BodegaId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        FechaVencimiento = c.DateTime(),
                        ProveedorId = c.Int(),
                        Lote = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observaciones = c.String(),
                    })
                .PrimaryKey(t => t.MovimientoInventarioId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId, cascadeDelete: true)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .Index(t => t.BodegaId)
                .Index(t => t.MaterialId);
            
            CreateTable(
                "dbo.SolicitudPedidoDF",
                c => new
                    {
                        SolicitudId = c.String(nullable: false, maxLength: 15),
                        Correlativo = c.String(),
                        Fecha = c.DateTime(),
                        DepartamentoId = c.Int(nullable: false),
                        DepartamentoSeccionId = c.Int(nullable: false),
                        Tipo = c.String(maxLength: 3),
                        Solicito = c.String(),
                        Elaboro = c.String(),
                        JefeDepartamento = c.String(),
                        Gerente = c.String(),
                        Director = c.String(),
                        Observaciones = c.String(),
                        FechaImpresion = c.String(),
                        Estatus = c.String(),
                    })
                .PrimaryKey(t => t.SolicitudId);
            
            CreateTable(
                "dbo.SolicitudPedidoDT",
                c => new
                    {
                        SolicitudPedidoDTId = c.Int(nullable: false, identity: true),
                        SolicitudId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        MaterialCodigo = c.String(),
                        Cantidad = c.Decimal(precision: 18, scale: 2),
                        UnidadMedida = c.String(),
                        Precio = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SolicitudPedidoDTId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.SolicitudPedidoDF", t => t.SolicitudId)
                .Index(t => t.SolicitudId)
                .Index(t => t.MaterialId);
            
            DropTable("dbo.Proveedor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        Nit = c.String(nullable: false, maxLength: 10),
                        ProveedorId = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 40),
                        Direccion = c.String(nullable: false, maxLength: 40),
                        Telefono = c.String(maxLength: 19),
                        Contacto = c.String(maxLength: 30),
                        DiasCredito = c.Int(),
                        Email = c.String(maxLength: 25),
                        Banco1 = c.String(maxLength: 20),
                        Cuenta1 = c.String(maxLength: 20),
                        Banco2 = c.String(maxLength: 20),
                        Cuenta2 = c.String(maxLength: 20),
                        Banco3 = c.String(maxLength: 20),
                        Cuenta3 = c.String(maxLength: 20),
                        PersonId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Nit);
            
            DropForeignKey("dbo.SolicitudPedidoDT", "SolicitudId", "dbo.SolicitudPedidoDF");
            DropForeignKey("dbo.SolicitudPedidoDT", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.MovimientoInventarios", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.MovimientoInventarios", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.IngresosInventarioDF", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.DespachosInventarioDT", "DespachoInventarioDF_DespachoInventarioId", "dbo.DespachosInventarioDF");
            DropForeignKey("dbo.DespachosInventarioDT", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.DespachosInventarioDF", "DestinoId", "dbo.Destinos");
            DropForeignKey("dbo.DespachosInventarioDF", "BodegaId", "dbo.Bodegas");
            DropIndex("dbo.SolicitudPedidoDT", new[] { "MaterialId" });
            DropIndex("dbo.SolicitudPedidoDT", new[] { "SolicitudId" });
            DropIndex("dbo.MovimientoInventarios", new[] { "MaterialId" });
            DropIndex("dbo.MovimientoInventarios", new[] { "BodegaId" });
            DropIndex("dbo.IngresosInventarioDF", new[] { "BodegaId" });
            DropIndex("dbo.DespachosInventarioDT", new[] { "DespachoInventarioDF_DespachoInventarioId" });
            DropIndex("dbo.DespachosInventarioDT", new[] { "MaterialId" });
            DropIndex("dbo.DespachosInventarioDF", new[] { "BodegaId" });
            DropIndex("dbo.DespachosInventarioDF", new[] { "DestinoId" });
            DropTable("dbo.SolicitudPedidoDT");
            DropTable("dbo.SolicitudPedidoDF");
            DropTable("dbo.MovimientoInventarios");
            DropTable("dbo.IngresosInventarioDF");
            DropTable("dbo.DespachosInventarioDT");
            DropTable("dbo.DespachosInventarioDF");
            CreateIndex("dbo.Proveedor", "PersonId");
            CreateIndex("dbo.Proveedor", "Nit", unique: true);
            AddForeignKey("dbo.Proveedor", "PersonId", "dbo.Personas", "PersonId");
        }
    }
}
