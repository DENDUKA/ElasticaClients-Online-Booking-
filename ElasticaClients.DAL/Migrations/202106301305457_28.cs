namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _28 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionStatusId", "dbo.SubscriptionStatus");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionStatusId" });
            AddColumn("dbo.Subscriptions", "StatusId", c => c.Int(nullable: false));
            DropColumn("dbo.Subscriptions", "SubscriptionStatusId");
            DropTable("dbo.SubscriptionStatus");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.SubscriptionStatus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Subscriptions", "SubscriptionStatusId", c => c.Int(nullable: false));
            DropColumn("dbo.Subscriptions", "StatusId");
            CreateIndex("dbo.Subscriptions", "SubscriptionStatusId");
            AddForeignKey("dbo.Subscriptions", "SubscriptionStatusId", "dbo.SubscriptionStatus", "Id", cascadeDelete: true);
        }
    }
}
