namespace N01609602_ShreyPatel_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Collaborators : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        CollaboratorId = c.Int(nullable: false, identity: true),
                        CollaboratorFirstName = c.String(),
                        CollaboratorLastName = c.String(),
                        CollaboratorEmail = c.String(),
                        CollaboratorRole = c.String(),
                    })
                .PrimaryKey(t => t.CollaboratorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Collaborators");
        }
    }
}
