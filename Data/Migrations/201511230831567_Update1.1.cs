namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update11 : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Comments", "DateCreated", c => c.DateTime(nullable: false));
			AddColumn("dbo.Comments", "DateModified", c => c.DateTime());
			AddColumn("dbo.Topics", "DateCreated", c => c.DateTime(nullable: false));
			AddColumn("dbo.Topics", "DateModified", c => c.DateTime());
        }
        
        public override void Down()
        {
			DropColumn("dbo.Topics", "DateModified");
			DropColumn("dbo.Topics", "DateCreated");
			DropColumn("dbo.Comments", "DateModified");
			DropColumn("dbo.Comments", "DateCreated");
        }
    }
}
