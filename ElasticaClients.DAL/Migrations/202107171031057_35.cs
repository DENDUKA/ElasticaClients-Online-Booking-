namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "Duration", c => c.Time(nullable: false, precision: 7));
        }

        public override void Down()
        {
            DropColumn("dbo.Trainings", "Duration");
        }
    }
}
