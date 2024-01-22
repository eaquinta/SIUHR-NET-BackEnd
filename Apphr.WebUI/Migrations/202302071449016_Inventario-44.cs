namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario44 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SHDAdministraciones",
                c => new
                    {
                        AdministracionId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 10),
                        Descripcion = c.String(maxLength: 100),
                        AccessRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdministracionId);
            
            CreateTable(
                "dbo.SHDBodegas",
                c => new
                    {
                        BodegaId = c.Int(nullable: false, identity: true),
                        AdministracionId = c.Int(nullable: false),
                        Codigo = c.String(maxLength: 5),
                        Descripcion = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.BodegaId)
                .ForeignKey("dbo.SHDAdministraciones", t => t.AdministracionId, cascadeDelete: false)
                .Index(t => new { t.AdministracionId, t.Codigo }, unique: true, name: "IX_Bodega");
            
            CreateTable(
                "dbo.SHDContadores",
                c => new
                    {
                        AdministracionId = c.Int(nullable: false, identity: true),
                        Ingreso = c.Int(nullable: false),
                        Despacho = c.Int(nullable: false),
                        Devolucion = c.Int(nullable: false),
                        Traslado = c.Int(nullable: false),
                        Ajuste = c.Int(nullable: false),
                        Apertura = c.Int(nullable: false),
                        Proceso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdministracionId);
            
            CreateTable(
                "dbo.SHDExistenciaBodega",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdministracionId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        CodigoMaterial = c.String(maxLength: 14),
                        CodigiMaterial = c.String(maxLength: 5),
                        BodegaId = c.Int(nullable: false),
                        CodigoBodega = c.String(maxLength: 5),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Minimo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Maximo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        STOCK = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CANSER = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VALSER = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BODSER = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SHDAdministraciones", t => t.AdministracionId, cascadeDelete: false)
                .ForeignKey("dbo.SHDBodegas", t => t.BodegaId, cascadeDelete: false)
                .ForeignKey("dbo.SHRMateriales", t => t.MaterialId, cascadeDelete: false)
                .Index(t => new { t.AdministracionId, t.CodigoMaterial, t.CodigiMaterial, t.CodigoBodega }, unique: true, name: "IX_ExistenciaBodega")
                .Index(t => t.MaterialId)
                .Index(t => t.BodegaId);
            
            CreateTable(
                "dbo.SHRMateriales",
                c => new
                    {
                        MaterialId = c.Int(nullable: false, identity: true),
                        Codigo = c.String(maxLength: 14),
                        Codigi = c.String(maxLength: 5),
                        Descripcion = c.String(maxLength: 254),
                        UnidadMedida = c.String(maxLength: 10),
                        Producto = c.String(maxLength: 30),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Minimo = c.Decimal(precision: 18, scale: 2),
                        Maximo = c.Decimal(precision: 18, scale: 2),
                        Ojo = c.String(maxLength: 1),
                        AlternoDe = c.String(maxLength: 14),
                        Bres = c.String(maxLength: 1),
                        FechaCreacion = c.DateTime(nullable: false),
                        Usuario = c.String(maxLength: 3),
                        Estatus = c.String(maxLength: 1),
                        VigenciaDe = c.DateTime(),
                        VigenciaA = c.DateTime(),
                        UsoBodega = c.String(maxLength: 5),
                        UsoServicio = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.MaterialId)
                .Index(t => new { t.Codigo, t.Codigi }, unique: true, name: "IX_Material");
            
            CreateTable(
                "dbo.SHDExistenciasLote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdministracionId = c.Int(nullable: false),
                        IdMaterial = c.Int(nullable: false),
                        CodigoMaterial = c.String(maxLength: 14),
                        CodigiMaterial = c.String(maxLength: 5),
                        CodigoBodega = c.String(maxLength: 5),
                        Vence = c.DateTime(),
                        TARJE = c.String(maxLength: 7),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UltimoPrecio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lote = c.String(maxLength: 15),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SHDAdministraciones", t => t.AdministracionId, cascadeDelete: false)
                .ForeignKey("dbo.SHRMateriales", t => t.IdMaterial, cascadeDelete: false)
                .Index(t => new { t.AdministracionId, t.CodigoMaterial, t.CodigiMaterial, t.CodigoBodega, t.Vence }, unique: true, name: "IX_ExistenciaLote")
                .Index(t => t.IdMaterial);
            
            CreateTable(
                "dbo.SHDExistenciaTotal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdministracionId = c.Int(nullable: false),
                        IdMaterial = c.Int(nullable: false),
                        CodigoMaterial = c.String(maxLength: 14),
                        CodigiMaterial = c.String(maxLength: 5),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SHDAdministraciones", t => t.AdministracionId, cascadeDelete: false)
                .ForeignKey("dbo.SHRMateriales", t => t.IdMaterial, cascadeDelete: false)
                .Index(t => new { t.AdministracionId, t.CodigoMaterial, t.CodigiMaterial }, unique: true, name: "IX_ExistenciaTotal")
                .Index(t => t.IdMaterial);
            
            CreateTable(
                "dbo.SHDMovimientosInventario",
                c => new
                    {
                        MovimientoInventarioId = c.Int(nullable: false, identity: true),
                        AdministracionId = c.Int(nullable: false),
                        Tipo = c.String(maxLength: 3),
                        Documento = c.Int(nullable: false),
                        Correlativo = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        MaterialCodigo = c.String(maxLength: 14),
                        MaterialCodigi = c.String(maxLength: 5),
                        BodegaCodigo = c.String(maxLength: 5),
                        BodegaId = c.Int(nullable: false),
                        Vence = c.DateTime(),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observacion = c.String(),
                        FechaCreado = c.DateTime(nullable: false),
                        FechaMovimiento = c.DateTime(nullable: false),
                        CantidadAnterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorAnterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MARCA = c.String(),
                        OPER = c.String(),
                        DocumentoIngreso = c.String(maxLength: 10),
                        Renglon = c.String(maxLength: 3),
                        ProveedorNit = c.String(maxLength: 10),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.MovimientoInventarioId)
                .ForeignKey("dbo.SHDAdministraciones", t => t.AdministracionId, cascadeDelete: false)
                .ForeignKey("dbo.SHDBodegas", t => t.BodegaId, cascadeDelete: false)
                .ForeignKey("dbo.SHRMateriales", t => t.MaterialId, cascadeDelete: false)
                .Index(t => t.AdministracionId)
                .Index(t => t.MaterialId)
                .Index(t => t.BodegaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SHDMovimientosInventario", "MaterialId", "dbo.SHRMateriales");
            DropForeignKey("dbo.SHDMovimientosInventario", "BodegaId", "dbo.SHDBodegas");
            DropForeignKey("dbo.SHDMovimientosInventario", "AdministracionId", "dbo.SHDAdministraciones");
            DropForeignKey("dbo.SHDExistenciaTotal", "IdMaterial", "dbo.SHRMateriales");
            DropForeignKey("dbo.SHDExistenciaTotal", "AdministracionId", "dbo.SHDAdministraciones");
            DropForeignKey("dbo.SHDExistenciasLote", "IdMaterial", "dbo.SHRMateriales");
            DropForeignKey("dbo.SHDExistenciasLote", "AdministracionId", "dbo.SHDAdministraciones");
            DropForeignKey("dbo.SHDExistenciaBodega", "MaterialId", "dbo.SHRMateriales");
            DropForeignKey("dbo.SHDExistenciaBodega", "BodegaId", "dbo.SHDBodegas");
            DropForeignKey("dbo.SHDExistenciaBodega", "AdministracionId", "dbo.SHDAdministraciones");
            DropForeignKey("dbo.SHDBodegas", "AdministracionId", "dbo.SHDAdministraciones");
            DropIndex("dbo.SHDMovimientosInventario", new[] { "BodegaId" });
            DropIndex("dbo.SHDMovimientosInventario", new[] { "MaterialId" });
            DropIndex("dbo.SHDMovimientosInventario", new[] { "AdministracionId" });
            DropIndex("dbo.SHDExistenciaTotal", new[] { "IdMaterial" });
            DropIndex("dbo.SHDExistenciaTotal", "IX_ExistenciaTotal");
            DropIndex("dbo.SHDExistenciasLote", new[] { "IdMaterial" });
            DropIndex("dbo.SHDExistenciasLote", "IX_ExistenciaLote");
            DropIndex("dbo.SHRMateriales", "IX_Material");
            DropIndex("dbo.SHDExistenciaBodega", new[] { "BodegaId" });
            DropIndex("dbo.SHDExistenciaBodega", new[] { "MaterialId" });
            DropIndex("dbo.SHDExistenciaBodega", "IX_ExistenciaBodega");
            DropIndex("dbo.SHDBodegas", "IX_Bodega");
            DropTable("dbo.SHDMovimientosInventario");
            DropTable("dbo.SHDExistenciaTotal");
            DropTable("dbo.SHDExistenciasLote");
            DropTable("dbo.SHRMateriales");
            DropTable("dbo.SHDExistenciaBodega");
            DropTable("dbo.SHDContadores");
            DropTable("dbo.SHDBodegas");
            DropTable("dbo.SHDAdministraciones");
        }
    }
}
