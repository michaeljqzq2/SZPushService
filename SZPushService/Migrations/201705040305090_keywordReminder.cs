namespace SZPushService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keywordReminder : DbMigration
    {
        public override void Up()
        {
            AddColumn("Keywords", "Remind", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
        }
    }
}
