namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Materiales", "Codigo", c => c.String(maxLength: 15));
            CreateIndex("dbo.Materiales", "Codigo", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Materiales", new[] { "Codigo" });
            AlterColumn("dbo.Materiales", "Codigo", c => c.String());
        }
    }
}
