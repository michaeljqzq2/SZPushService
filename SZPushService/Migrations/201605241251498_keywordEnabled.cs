namespace SZPushService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keywordEnabled : DbMigration
    {
        public override void Up()
        {
            //AddColumn("Keywords", "IsEnabled", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
        }
    }
}
