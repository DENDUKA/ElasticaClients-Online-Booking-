namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TrainingItems", name: "SubscriptionBuyId", newName: "SubscriptionId");
            RenameIndex(table: "dbo.TrainingItems", name: "IX_SubscriptionBuyId", newName: "IX_SubscriptionId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.TrainingItems", name: "IX_SubscriptionId", newName: "IX_SubscriptionBuyId");
            RenameColumn(table: "dbo.TrainingItems", name: "SubscriptionId", newName: "SubscriptionBuyId");
        }
    }
}
