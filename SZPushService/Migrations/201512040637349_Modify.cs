namespace SZPushService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Source", c => c.String());
            AddColumn("dbo.Messages", "Html", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Html");
            DropColumn("dbo.Messages", "Source");
        }
    }
}
