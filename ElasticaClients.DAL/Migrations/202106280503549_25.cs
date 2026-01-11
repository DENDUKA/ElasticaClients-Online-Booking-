namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "TrainingCount", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "TrainingCount");
        }
    }
}
