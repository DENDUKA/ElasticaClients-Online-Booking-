namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "SeatsTaken", c => c.Int(nullable: false));
            DropColumn("dbo.Trainings", "SeatsLeft");
        }

        public override void Down()
        {
            AddColumn("dbo.Trainings", "SeatsLeft", c => c.Int(nullable: false));
            DropColumn("dbo.Trainings", "SeatsTaken");
        }
    }
}
