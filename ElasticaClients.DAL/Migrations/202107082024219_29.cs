namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "Status", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TrainingItems", "Status");
        }
    }
}
