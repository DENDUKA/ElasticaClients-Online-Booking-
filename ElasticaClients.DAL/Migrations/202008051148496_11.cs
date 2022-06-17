namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Phone", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Phone");
        }
    }
}
