namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "BuyDate", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "BuyDate");
        }
    }
}
