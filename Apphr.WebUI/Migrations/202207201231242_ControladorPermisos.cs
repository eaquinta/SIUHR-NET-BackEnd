namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControladorPermisos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControladorPermisos",
                c => new
                    {
                        ControladorPermisoId = c.Int(nullable: false, identity: true),
                        AccionId = c.Int(nullable: false),
                        Nombre = c.String(),
                        Permiso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControladorPermisoId)
                .ForeignKey("dbo.Controladores", t => t.AccionId, cascadeDelete: true)
                .Index(t => t.AccionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControladorPermisos", "AccionId", "dbo.Controladores");
            DropIndex("dbo.ControladorPermisos", new[] { "AccionId" });
            DropTable("dbo.ControladorPermisos");
        }
    }
}
