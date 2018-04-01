using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltaDigital.DbMigrator.Configurations;
using AltaDigital.DbMigrator.Exceptions;
using Xunit;

namespace AltaDigital.DbMigrator.Tests
{
    public class MigrationContextConfigTests
    {
        private static class DataSource
        {
            public static IEnumerable<object[]> Data => new[]
            {
                new object[]
                {
                    "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;",
                    new Dictionary<string, string>(new []
                    {
                        new KeyValuePair<string, string>("Server", "myServerAddress"),
                        new KeyValuePair<string, string>("Database", "myDataBase"),
                        new KeyValuePair<string, string>("Uid", "myUsername"),
                        new KeyValuePair<string, string>("Pwd", "myPassword")
                    })
                },
                new object[]
                {
                    "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;",
                    new Dictionary<string, string>(new []
                    {
                        new KeyValuePair<string, string>("Server", "myServerAddress"),
                        new KeyValuePair<string, string>("Database", "myDataBase"),
                        new KeyValuePair<string, string>("User Id", "myUsername"),
                        new KeyValuePair<string, string>("Password", "myPassword")
                    })
                },
                new object[]
                {
                    "User ID=root;Password=q1w2e3*%hjU;Host=localhost;Port=5432;Database=myDataBase;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;",
                    new Dictionary<string, string>(new []
                    {
                        new KeyValuePair<string, string>("User ID", "root"),
                        new KeyValuePair<string, string>("Password", "q1w2e3*%hjU"),
                        new KeyValuePair<string, string>("Host", "localhost"),
                        new KeyValuePair<string, string>("Port", "5432"),
                        new KeyValuePair<string, string>("Database", "myDataBase"),
                        new KeyValuePair<string, string>("Pooling", "true"),
                        new KeyValuePair<string, string>("Min Pool Size", "0"),
                        new KeyValuePair<string, string>("Max Pool Size", "100"),
                        new KeyValuePair<string, string>("Connection Lifetime", "0")
                    })
                },
                new object[]
                {
                    "Server=.\\SQLExpress;AttachDbFilename=C:\\MyFolder\\MyDataFile.mdf;Database=dbname;Trusted Connection=Yes;",
                    new Dictionary<string, string>(new []
                    {
                        new KeyValuePair<string, string>("Server", ".\\SQLExpress"),
                        new KeyValuePair<string, string>("AttachDbFilename", "C:\\MyFolder\\MyDataFile.mdf"),
                        new KeyValuePair<string, string>("Database", "dbname"),
                        new KeyValuePair<string, string>("Trusted Connection", "Yes")
                    })
                }
            };
        }

        [Theory]
        [MemberData(nameof(DataSource.Data), MemberType = typeof(DataSource))]
        public void SuccessParseConnectionStringTest(string connectionString, Dictionary<string, string> claims)
        {
            Assert.Equal(claims, new MigrationContextConfig(connectionString, true).ConnectionClaims);
        }

        [Fact]
        public async Task ThrowsExceptionOnNullConnectionStringTest()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => Task.Run(() => new MigrationContextConfig(null, true)));
        }

        [Fact]
        public async Task ThrowsExceptionOnEmptyConnectionStringTest()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => Task.Run(() => new MigrationContextConfig(" ", true)));
        }

        [Fact]
        public async Task ThrowsExceptionOnInvalidConnectionStringTest()
        {
            await Assert.ThrowsAsync<MigrationContextException>(() => Task.Run(() => new MigrationContextConfig("invalid connection string", true).ConnectionClaims));
        }
    }
}
