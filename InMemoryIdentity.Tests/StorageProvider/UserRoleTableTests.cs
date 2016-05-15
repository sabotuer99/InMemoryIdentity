using InMemoryIdentity.StorageProvider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryIdentity.Tests.StorageProvider
{
    [TestFixture]
    class UserRolesTableTests
    {
        [SetUp]
        public void SetUp()
        {
            InMemoryContext.Init();
        }

        [Test]
        public void FindByUserId_UserIdValid_ReturnsListOfRoles()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.FindByUserId("1");

            //Assert
            Assert.AreEqual(3, result.Count());
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
        }

        [Test]
        public void Delete_UserIdValid_ReturnsDeleteCount()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("1");

            //Assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Delete_UserIdNotValid_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("100");

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Insert_UserNotAlreadyAssignedToRole_InsertsRoleForUser()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var user = new IdentityUser() {Id = "1"};

            //Act
            var result = sut.Insert(user, "roleId");

            //Assert
            Assert.AreEqual(1, db.userRoles.Count());
            Assert.AreEqual(1, db.userRoles["1"].Count());

        }

        [Test]
        public void Insert_UserHasSomeRoles_InsertsAdditionalRoleForUser()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };

            //Act
            var result = sut.Insert(user, "roleId");

            //Assert
            Assert.AreEqual(2, db.userRoles.Count());
            Assert.AreEqual(4, db.userRoles["1"].Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_UserAlreadyAssignedToRole_ThrowsArgumentException()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);
            var user = new IdentityUser() { Id = "1" };

            //Act
            var result = sut.Insert(user, "one");

            //Assert
            //Should throw exception
        }

        private InMemoryContext getFullDb()
        {
            var db = new InMemoryContext();
            db.userRoles["1"] = new List<string>();
            db.userRoles["1"].Add("one");
            db.userRoles["1"].Add("two");
            db.userRoles["1"].Add("three");

            db.userRoles["2"] = new List<string>();
            db.userRoles["2"].Add("one");
            db.userRoles["2"].Add("two");
            db.userRoles["2"].Add("three");

            return db;
        }

        private UserRolesTable getSut(InMemoryContext db)
        {
            return new UserRolesTable(db);
        }
    }
}
