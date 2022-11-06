namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario08 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SolicitudDespacho",
                c => new
                    {
                        SolicitudDespachoId = c.String(nullable: false, maxLength: 15),
                        Correlativo = c.String(),
                        Fecha = c.DateTime(),
                        DepartamentoId = c.Int(nullable: false),
                        SeccionId = c.Int(nullable: false),
                        Otros = c.String(),
                        Tipo = c.String(),
                        Solicito = c.String(),
                        Jefe = c.String(),
                        Gerente = c.String(),
                        Observaciones = c.String(),
                        NumeroLineas = c.Int(),
                        Estatus = c.String(),
                    })
                .PrimaryKey(t => t.SolicitudDespachoId)
                .ForeignKey("dbo.Destinos", t => t.DepartamentoId, cascadeDelete: false)
                .ForeignKey("dbo.Destinos", t => t.SeccionId, cascadeDelete: false)
                .Index(t => t.DepartamentoId)
                .Index(t => t.SeccionId);
            
            CreateTable(
                "dbo.SolicitudDespachoDT",
                c => new
                    {
                        SolicitudDespachoDTId = c.Int(nullable: false, identity: true),
                        SolicitudDespachoId = c.String(maxLength: 15),
                        MaterialId = c.Int(nullable: false),
                        Cantidad = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SolicitudDespachoDTId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.SolicitudDespacho", t => t.SolicitudDespachoId)
                .Index(t => t.SolicitudDespachoId)
                .Index(t => t.MaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SolicitudDespacho", "SeccionId", "dbo.Destinos");
            DropForeignKey("dbo.SolicitudDespachoDT", "SolicitudDespachoId", "dbo.SolicitudDespacho");
            DropForeignKey("dbo.SolicitudDespachoDT", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.SolicitudDespacho", "DepartamentoId", "dbo.Destinos");
            DropIndex("dbo.SolicitudDespachoDT", new[] { "MaterialId" });
            DropIndex("dbo.SolicitudDespachoDT", new[] { "SolicitudDespachoId" });
            DropIndex("dbo.SolicitudDespacho", new[] { "SeccionId" });
            DropIndex("dbo.SolicitudDespacho", new[] { "DepartamentoId" });
            DropTable("dbo.SolicitudDespachoDT");
            DropTable("dbo.SolicitudDespacho");
        }
    }
}
