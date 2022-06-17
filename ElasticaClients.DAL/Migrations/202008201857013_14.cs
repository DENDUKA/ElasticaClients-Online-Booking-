namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Password");
        }
    }
}
