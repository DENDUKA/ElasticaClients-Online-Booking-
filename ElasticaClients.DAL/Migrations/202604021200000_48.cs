namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _48 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppLogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CreatedAt = c.DateTime(nullable: false),
                    ActionType = c.String(),
                    ActorName = c.String(),
                    ClientName = c.String(),
                    TrainingInfo = c.String(),
                    PaymentType = c.String(),
                    Cost = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.AppLogs");
        }
    }
}
