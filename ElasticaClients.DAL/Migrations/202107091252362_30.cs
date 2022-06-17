namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "Name");
        }
    }
}
