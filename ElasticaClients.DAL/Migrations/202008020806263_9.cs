namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionStatus_Id", "dbo.SubscriptionStatus");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionStatus_Id" });
            RenameColumn(table: "dbo.Subscriptions", name: "SubscriptionStatus_Id", newName: "SubscriptionStatusId");
            AlterColumn("dbo.Subscriptions", "SubscriptionStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "SubscriptionStatusId");
            AddForeignKey("dbo.Subscriptions", "SubscriptionStatusId", "dbo.SubscriptionStatus", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionStatusId", "dbo.SubscriptionStatus");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionStatusId" });
            AlterColumn("dbo.Subscriptions", "SubscriptionStatusId", c => c.Int());
            RenameColumn(table: "dbo.Subscriptions", name: "SubscriptionStatusId", newName: "SubscriptionStatus_Id");
            CreateIndex("dbo.Subscriptions", "SubscriptionStatus_Id");
            AddForeignKey("dbo.Subscriptions", "SubscriptionStatus_Id", "dbo.SubscriptionStatus", "Id");
        }
    }
}
