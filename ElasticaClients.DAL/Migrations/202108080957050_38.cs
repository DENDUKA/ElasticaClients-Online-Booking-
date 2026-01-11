namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FreezeSubscriptionItems", "Description", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FreezeSubscriptionItems", "Description");
        }
    }
}
