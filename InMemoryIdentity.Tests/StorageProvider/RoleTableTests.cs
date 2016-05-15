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
    class RoleTableTests
    {
        [SetUp]
        public void SetUp()
        {
            InMemoryContext.Init();
        }

        [Test]
        public void Insert_RecordDoesntExist_RecordInserted()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");

            //Act
            sut.Insert(record);

            //Assert
            Assert.AreEqual(1, db.roles.Count);
            Assert.AreEqual(record, db.roles["1234"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_RecordAlreadyExists_ThrowArgumentException()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");
            db.roles.Add(record.Id, record);

            //Act
            sut.Insert(record);

            //Assert
            //Should throw exception
        }

        [Test]
        public void Delete_RecordExists_RecordDeleted()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");
            db.roles.Add(record.Id, record);

            //Act
            var count = sut.Delete(record.Id);

            //Assert
            Assert.AreEqual(0, db.roles.Count);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void Delete_RecordDoesntExists_Return0()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");
            db.roles.Add(record.Id, record);

            //Act
            var count = sut.Delete("4321");

            //Assert
            Assert.AreEqual(1, db.roles.Count);
            Assert.AreEqual(0, count);
        }

        [Test]
        public void Update_RecordExists_RecordUpdated()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");
            var updated = testRole("1234", "updated");
            db.roles.Add(record.Id, record);

            //Act
            sut.Update(updated);

            //Assert
            Assert.AreEqual("updated", db.roles["1234"].Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_RecordDoesntExist_ThrowArgumentException()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            var record = testRole("1234", "test");
            var updated = testRole("1234", "updated");

            //Act
            sut.Update(updated);

            //Assert
            //Should throw exception
        }

        [Test]
        public void GetRoleName_RoleExists_ReturnsName()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleName("1");

            //Assert
            Assert.AreEqual("one", result);        
        }

        [Test]
        public void GetRoleName_RoleDoesntExist_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleName("10");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetRoleId_RoleExists_ReturnsId()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleId("one");

            //Assert
            Assert.AreEqual("1", result);
        }

        [Test]
        public void GetRoleId_RoleDoesntExist_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleId("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetRoleByName_RoleExists_ReturnsRole()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleByName("one");

            //Assert
            Assert.AreEqual("one", result.Name);
            Assert.AreEqual("1", result.Id);
        }

        [Test]
        public void GetRoleByName_RoleDoesntExist_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleByName("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetRoleById_RoleExists_ReturnsRole()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleById("1");

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("one", result.Name);
        }

        [Test]
        public void GetRoleById_RoleDoesntExist_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetRoleById("10");

            //Assert
            Assert.Null(result);
        }

        private InMemoryContext getFullDb()
        {
            var db = new InMemoryContext();
            db.roles.Add("1", new IdentityRole() { Id = "1", Name = "one" });
            db.roles.Add("2", new IdentityRole() { Id = "2", Name = "two" });
            db.roles.Add("3", new IdentityRole() { Id = "3", Name = "three" });
            db.roles.Add("4", new IdentityRole() { Id = "4", Name = "four" });

            return db;
        }

        private RoleTable getSut(InMemoryContext db)
        {
            return new RoleTable(db);
        }

        private IdentityRole testRole(string id, string name)
        {
            return new IdentityRole() { Id = id, Name = name };
        }
    }
}
