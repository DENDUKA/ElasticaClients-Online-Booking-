namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incomes", "GymId", "dbo.Gyms");
            DropIndex("dbo.Incomes", new[] { "GymId" });
            DropTable("dbo.Incomes");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Incomes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Cost = c.Int(nullable: false),
                    GymId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.Incomes", "GymId");
            AddForeignKey("dbo.Incomes", "GymId", "dbo.Gyms", "Id", cascadeDelete: true);
        }
    }
}