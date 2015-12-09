namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interests", "AuthorID", c => c.Int(nullable: false));
            //AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Users", "IsActive");
            DropColumn("dbo.Interests", "AuthorID");
        }
    }
}
