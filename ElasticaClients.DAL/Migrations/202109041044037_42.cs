namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _42 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "StatusPayId", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TrainingItems", "StatusPayId");
        }
    }
}
