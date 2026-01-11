namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _34 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trainings", "StatusId", "dbo.Status");
            DropIndex("dbo.Trainings", new[] { "StatusId" });
            DropTable("dbo.Status");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Status",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.Trainings", "StatusId");
            AddForeignKey("dbo.Trainings", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
        }
    }
}
