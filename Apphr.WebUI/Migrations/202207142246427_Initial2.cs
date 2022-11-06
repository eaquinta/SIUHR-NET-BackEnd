namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Facilitadors", new[] { "PersonId" });
            CreateTable(
                "dbo.Controladores",
                c => new
                    {
                        AccionId = c.Int(nullable: false, identity: true),
                        Area = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        Detalle = c.String(),
                    })
                .PrimaryKey(t => t.AccionId);
            
            CreateTable(
                "dbo.ControladorRolAsignaciones",
                c => new
                    {
                        AccionId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccionId, t.RoleId })
                .ForeignKey("dbo.Controladores", t => t.AccionId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccionId)
                .Index(t => t.RoleId);
            
            CreateIndex("dbo.Facilitadors", "PersonId", unique: true, name: "IX_PersonaUnique");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControladorRolAsignaciones", "RoleId", "dbo.AppRole");
            DropForeignKey("dbo.ControladorRolAsignaciones", "AccionId", "dbo.Controladores");
            DropIndex("dbo.Facilitadors", "IX_PersonaUnique");
            DropIndex("dbo.ControladorRolAsignaciones", new[] { "RoleId" });
            DropIndex("dbo.ControladorRolAsignaciones", new[] { "AccionId" });
            DropTable("dbo.ControladorRolAsignaciones");
            DropTable("dbo.Controladores");
            CreateIndex("dbo.Facilitadors", "PersonId");
        }
    }
}
