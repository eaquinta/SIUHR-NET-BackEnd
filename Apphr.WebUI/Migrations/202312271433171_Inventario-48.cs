namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario48 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppPermissions",
                c => new
                    {
                        PermissionId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.PermissionId);
            
            CreateTable(
                "dbo.AppRolePermissions",
                c => new
                    {
                        AppRoleId = c.Int(nullable: false),
                        AppPermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppRoleId, t.AppPermissionId })
                .ForeignKey("dbo.AppPermissions", t => t.AppPermissionId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.AppRoleId, cascadeDelete: true)
                .Index(t => t.AppRoleId)
                .Index(t => t.AppPermissionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppRolePermissions", "AppRoleId", "dbo.AppRole");
            DropForeignKey("dbo.AppRolePermissions", "AppPermissionId", "dbo.AppPermissions");
            DropIndex("dbo.AppRolePermissions", new[] { "AppPermissionId" });
            DropIndex("dbo.AppRolePermissions", new[] { "AppRoleId" });
            DropTable("dbo.AppRolePermissions");
            DropTable("dbo.AppPermissions");
        }
    }
}
