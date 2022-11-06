namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Materiales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Materiales",
                c => new
                    {
                        MaterialId = c.Int(nullable: false, identity: true),
                        RenglonCodigo = c.String(),
                        GrupoCodigo = c.String(),
                        Codigo = c.String(),
                        UnidadMedida = c.String(),
                        Descripcion = c.String(),
                        TipoContrato = c.String(),
                        VigenciaDe = c.DateTime(),
                        VigenciaA = c.DateTime(),
                        Producto = c.String(),
                        Precio = c.Decimal(precision: 18, scale: 2),
                        Minimo = c.Decimal(precision: 18, scale: 2),
                        Maximo = c.Decimal(precision: 18, scale: 2),
                        Estatus = c.String(),
                        SigesCodigo = c.String(),
                    })
                .PrimaryKey(t => t.MaterialId);
            
            AddColumn("dbo.Destinos", "JefeServicio", c => c.String());
            DropColumn("dbo.Destinos", "JefeServocio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Destinos", "JefeServocio", c => c.String());
            DropColumn("dbo.Destinos", "JefeServicio");
            DropTable("dbo.Materiales");
        }
    }
}
