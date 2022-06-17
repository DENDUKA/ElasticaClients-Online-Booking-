namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _37 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FreezeSubscriptionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscriptions", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.SubscriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FreezeSubscriptionItems", "SubscriptionId", "dbo.Subscriptions");
            DropIndex("dbo.FreezeSubscriptionItems", new[] { "SubscriptionId" });
            DropTable("dbo.FreezeSubscriptionItems");
        }
    }
}
