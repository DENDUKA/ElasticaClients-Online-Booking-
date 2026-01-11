namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "IsTrial", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.TrainingItems", "IsTrial");
        }
    }
}
