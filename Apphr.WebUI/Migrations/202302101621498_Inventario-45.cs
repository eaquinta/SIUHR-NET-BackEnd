namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario45 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SHRDestinoes",
                c => new
                    {
                        DestinoId = c.Int(nullable: false, identity: true),
                        Codigo = c.String(maxLength: 5),
                        Descripcion = c.String(maxLength: 30),
                        ADMINI = c.String(maxLength: 2),
                        JefeServicio = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.DestinoId);
            
            AddColumn("dbo.SHDMovimientosInventario", "DestinoId", c => c.Int());
            AddColumn("dbo.SHDMovimientosInventario", "DestinoCodigo", c => c.String(maxLength: 5));
            AddColumn("dbo.SHDMovimientosInventario", "Solicitud", c => c.String(maxLength: 10));
            CreateIndex("dbo.SHDMovimientosInventario", "DestinoId");
            AddForeignKey("dbo.SHDMovimientosInventario", "DestinoId", "dbo.SHRDestinoes", "DestinoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SHDMovimientosInventario", "DestinoId", "dbo.SHRDestinoes");
            DropIndex("dbo.SHDMovimientosInventario", new[] { "DestinoId" });
            DropColumn("dbo.SHDMovimientosInventario", "Solicitud");
            DropColumn("dbo.SHDMovimientosInventario", "DestinoCodigo");
            DropColumn("dbo.SHDMovimientosInventario", "DestinoId");
            DropTable("dbo.SHRDestinoes");
        }
    }
}
