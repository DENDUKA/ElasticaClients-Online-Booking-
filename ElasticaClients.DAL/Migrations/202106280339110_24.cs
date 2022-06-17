namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "Cost", c => c.Int(nullable: false));
            AddColumn("dbo.Subscriptions", "ActiveDays", c => c.Int(nullable: false));
            AddColumn("dbo.Subscriptions", "ByCash", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "ByCash");
            DropColumn("dbo.Subscriptions", "ActiveDays");
            DropColumn("dbo.Subscriptions", "Cost");
        }
    }
}
