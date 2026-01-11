namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Subscriptions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Price = c.Int(nullable: false),
                    CountTrainings = c.Int(nullable: false),
                    AccountId = c.Int(nullable: false),
                    GymId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Gyms", t => t.GymId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.GymId);

            CreateTable(
                "dbo.Gyms",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Trainings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StartTime = c.DateTime(nullable: false),
                    Seats = c.Int(nullable: false),
                    GymId = c.Int(nullable: false),
                    StatusId = c.Int(nullable: false),
                    TrainerId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gyms", t => t.GymId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.GymId)
                .Index(t => t.StatusId)
                .Index(t => t.TrainerId);

            CreateTable(
                "dbo.Status",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TrainingItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SubscriptionId = c.Int(nullable: false),
                    TrainingId = c.Int(nullable: false),
                    Account_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscriptions", t => t.SubscriptionId, cascadeDelete: true)
                .ForeignKey("dbo.Trainings", t => t.TrainingId, cascadeDelete: false)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.SubscriptionId)
                .Index(t => t.TrainingId)
                .Index(t => t.Account_Id);

            AddColumn("dbo.Accounts", "RoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "RoleId");
            AddForeignKey("dbo.Accounts", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.TrainingItems", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Subscriptions", "GymId", "dbo.Gyms");
            DropForeignKey("dbo.TrainingItems", "TrainingId", "dbo.Trainings");
            DropForeignKey("dbo.TrainingItems", "SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.Trainings", "TrainerId", "dbo.Accounts");
            DropForeignKey("dbo.Trainings", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Trainings", "GymId", "dbo.Gyms");
            DropForeignKey("dbo.Subscriptions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropIndex("dbo.TrainingItems", new[] { "Account_Id" });
            DropIndex("dbo.TrainingItems", new[] { "TrainingId" });
            DropIndex("dbo.TrainingItems", new[] { "SubscriptionId" });
            DropIndex("dbo.Trainings", new[] { "TrainerId" });
            DropIndex("dbo.Trainings", new[] { "StatusId" });
            DropIndex("dbo.Trainings", new[] { "GymId" });
            DropIndex("dbo.Subscriptions", new[] { "GymId" });
            DropIndex("dbo.Subscriptions", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropColumn("dbo.Accounts", "RoleId");
            DropTable("dbo.TrainingItems");
            DropTable("dbo.Status");
            DropTable("dbo.Trainings");
            DropTable("dbo.Gyms");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Roles");
        }
    }
}
