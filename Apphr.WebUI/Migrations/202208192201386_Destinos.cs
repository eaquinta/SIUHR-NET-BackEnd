namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Destinos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Destinos",
                c => new
                    {
                        DestinoId = c.Int(nullable: false, identity: true),
                        Codigo = c.String(maxLength: 10),
                        Descripcion = c.String(),
                        ADMINI = c.String(),
                        JefeServocio = c.String(),
                    })
                .PrimaryKey(t => t.DestinoId)
                .Index(t => t.Codigo, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Destinos", new[] { "Codigo" });
            DropTable("dbo.Destinos");
        }
    }
}
