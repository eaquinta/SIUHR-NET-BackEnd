namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSQLScript : DbMigration
    {
        public override void Up()
        {
            string sqlResName = typeof(InitialSQLScript).Namespace + ".202207061805386_InitialSQLScript.sql";
            this.SqlResource(sqlResName);
        }
        
        public override void Down()
        {
        }
    }
}
