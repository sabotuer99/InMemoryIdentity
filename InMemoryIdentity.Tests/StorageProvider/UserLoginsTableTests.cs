using InMemoryIdentity.StorageProvider;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryIdentity.Tests.StorageProvider
{
    [TestFixture]
    class UserLoginsTableTests
    {
        [SetUp]
        public void SetUp()
        {
            InMemoryContext.Init();
        }

        [Test]
        public void Insert_UserDoesNotExist_InsertsLoginInfo()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var user = new IdentityUser() {Id = "1"};
            var info = new UserLoginInfo("test", "test");

            //Act
            var result = sut.Insert(user, info);

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(1, db.userLogins.Count());
        }

        [Test]
        public void Insert_UserExistsNewLoginInfo_InsertsLoginInfo()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };
            var info = new UserLoginInfo("test", "test");

            //Act
            var result = sut.Insert(user, info);

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(3, db.userLogins[user.Id].Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_LoginInfoAlreadyExists_ThrowsException()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };
            var info = new UserLoginInfo("google", "google1");

            //Act
            var result = sut.Insert(user, info);

            //Assert
            //Should throw exception
        }

        [Test]
        public void Delete_GivenGoodUserId_ReturnsDeletedCount()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("1");

            //Assert
            Assert.AreEqual(2, result);
            Assert.False(db.userLogins.ContainsKey("1"));
        }

        [Test]
        public void Delete_UserIdNotExist_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("10");

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Delete_GivenUserIdAndLoginInfoValid_DeletesLoginInfo()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };
            var info = new UserLoginInfo("google", "google1");

            //Act
            var result = sut.Delete(user, info);

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(1, db.userLogins[user.Id].Count());
        }

        [Test]
        public void Delete_UserValidButLoginInfoNot_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };
            var info = new UserLoginInfo("test", "test");

            //Act
            var result = sut.Delete(user, info);

            //Assert
            Assert.AreEqual(0, result);
            Assert.AreEqual(2, db.userLogins[user.Id].Count());
        }

        [Test]
        public void Delete_UserNotFoundLoginInfoIrrelevant_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "100" };
            var info = new UserLoginInfo("test", "test");

            //Act
            var result = sut.Delete(user, info);

            //Assert
            Assert.AreEqual(0, result);
            Assert.False(db.userLogins.ContainsKey("100"));
        }

        [Test]
        public void FindUserIdByLogin_LoginInfoValid_ReturnsUserId()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var info = new UserLoginInfo("google", "google1");

            //Act
            var result = sut.FindUserIdByLogin(info);

            //Assert
            Assert.AreEqual("1", result);
        }

        [Test]
        public void FindUserIdByLogin_LoginInfoNotValid_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var info = new UserLoginInfo("test", "test");

            //Act
            var result = sut.FindUserIdByLogin(info);

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void FindByUserId_UserIdValid_ReturnsListOfLogins()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.FindByUserId("1");

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void FindByUserId_UserIdNotValid_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.FindByUserId("100");

            //Assert
            Assert.Null(result);
            Assert.False(db.userLogins.ContainsKey("100"));
        }

        //
        //   HELPERS
        //
        private InMemoryContext getFullDb()
        {
            var db = new InMemoryContext();
            db.userLogins["1"] = new List<UserLoginInfo>();
            db.userLogins["1"].Add(new UserLoginInfo("google", "google1"));
            db.userLogins["1"].Add(new UserLoginInfo("facebook", "fb1"));

            db.userLogins["2"] = new List<UserLoginInfo>();
            db.userLogins["2"].Add(new UserLoginInfo("google", "google2"));
            db.userLogins["2"].Add(new UserLoginInfo("facebook", "fb2"));

            return db;
        }

        private UserLoginsTable getSut(InMemoryContext db)
        {
            return new UserLoginsTable(db);
        }
    }
}
