namespace AltaDigital.DbMigrator.Tests.Migrations
{
    public class Migration1 : MigrationBase
    {
        public override long Key => 20180127001500L;

        public override void Up(IMigrationAction action)
        {
            action.ExecuteSql("create table if not exists test1(id int, name text);");
        }

        public override void Down(IMigrationAction action)
        {
            action.ExecuteSql("drop table test1;");
        }
    }
}
