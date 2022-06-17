namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Subscriptions", "SubscriptionStatus_Id", c => c.Int());
            CreateIndex("dbo.Subscriptions", "SubscriptionStatus_Id");
            AddForeignKey("dbo.Subscriptions", "SubscriptionStatus_Id", "dbo.SubscriptionStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionStatus_Id", "dbo.SubscriptionStatus");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionStatus_Id" });
            DropColumn("dbo.Subscriptions", "SubscriptionStatus_Id");
            DropTable("dbo.SubscriptionStatus");
        }
    }
}
