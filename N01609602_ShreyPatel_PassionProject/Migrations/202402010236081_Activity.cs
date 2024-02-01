namespace N01609602_ShreyPatel_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ActivityName = c.String(),
                        ActivityDescription = c.String(),
                        ActivityPriority = c.String(),
                        ActivityStatus = c.String(),
                        ActivityDueDate = c.DateTime(nullable: false),
                        ActivityEstimates = c.String(),
                        ProjectId = c.Int(nullable: false),
                        CollaboratorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Collaborators", t => t.CollaboratorId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.CollaboratorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Activities", "CollaboratorId", "dbo.Collaborators");
            DropIndex("dbo.Activities", new[] { "CollaboratorId" });
            DropIndex("dbo.Activities", new[] { "ProjectId" });
            DropTable("dbo.Activities");
        }
    }
}
