namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "YClientsId", c => c.Int(nullable: false));
            AddColumn("dbo.Trainings", "YClientId", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Trainings", "YClientId");
            DropColumn("dbo.TrainingItems", "YClientsId");
        }
    }
}
