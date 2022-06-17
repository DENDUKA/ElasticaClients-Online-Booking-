namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Subscriptions", newName: "SubscriptionBuys");
            RenameColumn(table: "dbo.TrainingItems", name: "SubscriptionId", newName: "SubscriptionBuyId");
            RenameIndex(table: "dbo.TrainingItems", name: "IX_SubscriptionId", newName: "IX_SubscriptionBuyId");
            DropColumn("dbo.SubscriptionBuys", "Name");
            DropColumn("dbo.SubscriptionBuys", "Price");
            DropColumn("dbo.SubscriptionBuys", "CountTrainings");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubscriptionBuys", "CountTrainings", c => c.Int(nullable: false));
            AddColumn("dbo.SubscriptionBuys", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.SubscriptionBuys", "Name", c => c.String());
            RenameIndex(table: "dbo.TrainingItems", name: "IX_SubscriptionBuyId", newName: "IX_SubscriptionId");
            RenameColumn(table: "dbo.TrainingItems", name: "SubscriptionBuyId", newName: "SubscriptionId");
            RenameTable(name: "dbo.SubscriptionBuys", newName: "Subscriptions");
        }
    }
}
