namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "Name", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Trainings", "Name");
        }
    }
}
