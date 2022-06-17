namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "SeatsLeft", c => c.Int(nullable: false));
            AddColumn("dbo.Trainings", "TrainerPay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainings", "TrainerPay");
            DropColumn("dbo.Trainings", "SeatsLeft");
        }
    }
}
