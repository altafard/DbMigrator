namespace AltaDigital.DbMigrator.Tests.Migrations
{
    public class Migration2 : MigrationBase
    {
        public override long Key => 20180127002200L;

        public override void Up(IMigrationAction action)
        {
            action.ExecuteSql("alter table test1 add column note text;");
        }

        public override void Down(IMigrationAction action)
        {
            action.ExecuteSql("alter table test1 drop column note;");
        }
    }
}
