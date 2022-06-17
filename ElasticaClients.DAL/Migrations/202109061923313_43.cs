namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Verified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Verified");
        }
    }
}
