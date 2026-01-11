namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "Phone", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.Accounts", "Phone", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
