namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prices", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prices", "Name", c => c.Int(nullable: false));
        }
    }
}
