using NUnit.Framework;
using Poker.Databases;
using Poker.Views;
using Uniform;
using Uniform.InMemory;

namespace Poker.Tests.Infrastructure
{
    [TestFixture]
    public class InMemoryDatabaseTest
    {
        [Test]
        public void test()
        {
            var db = new InMemoryDatabase();
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(UserView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, db));
            var viewDb = new ViewDatabase(uniform);
            viewDb.Users.Save(new UserView()
            {
                Id = "me1",
                UserName = "User Name"
            });
            var storedUser = db.GetCollection<UserView>(ViewCollections.Users).GetById("me1");
            Assert.AreEqual("me1", storedUser.Id);
            Assert.AreEqual("User Name", storedUser.UserName);

        }
    }
}