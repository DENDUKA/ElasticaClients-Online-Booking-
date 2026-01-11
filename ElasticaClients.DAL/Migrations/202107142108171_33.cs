namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "StatusId", c => c.Int(nullable: false));
            DropColumn("dbo.TrainingItems", "Status");
        }

        public override void Down()
        {
            AddColumn("dbo.TrainingItems", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.TrainingItems", "StatusId");
        }
    }
}
