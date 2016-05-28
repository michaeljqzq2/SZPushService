namespace SZPushService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Keywords",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Messages",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ArticleId = c.String(),
            //            Timestamp = c.DateTime(nullable: false),
            //            Title = c.String(),
            //            Detail = c.String(),
            //            Price = c.String(),
            //            Keyword = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Messages");
            //DropTable("dbo.Keywords");
        }
    }
}
