namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incomes", "Date");
        }
    }
}
