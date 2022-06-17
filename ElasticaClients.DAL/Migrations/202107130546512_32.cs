namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "ActivateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "ActivateDate");
        }
    }
}
