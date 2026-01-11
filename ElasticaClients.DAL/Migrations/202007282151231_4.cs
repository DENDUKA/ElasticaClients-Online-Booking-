namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SubscriptionBuys", newName: "Subscriptions");
            CreateTable(
                "dbo.Prices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.Int(nullable: false),
                    Cost = c.Int(nullable: false),
                    Count = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Subscriptions", "PriceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "PriceId");
            AddForeignKey("dbo.Subscriptions", "PriceId", "dbo.Prices", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "PriceId", "dbo.Prices");
            DropIndex("dbo.Subscriptions", new[] { "PriceId" });
            DropColumn("dbo.Subscriptions", "PriceId");
            DropTable("dbo.Prices");
            RenameTable(name: "dbo.Subscriptions", newName: "SubscriptionBuys");
        }
    }
}
