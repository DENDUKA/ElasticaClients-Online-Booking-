namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "TrainingsLeft", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "TrainingsLeft");
        }
    }
}
