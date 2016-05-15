using InMemoryIdentity.Models;
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
    class UserTableTests
    {
        [SetUp]
        public void SetUp()
        {
            InMemoryContext.Init();
        }


        [Test]
        public void GetUserName_ValidUserId_ReturnsName()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserName("1");

            //Assert
            Assert.AreEqual("test1", result);
        }

        [Test]
        public void GetUserName_InvalidUserId_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserName("100");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetUserId_ValidUserName_ReturnsId()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserId("test1");

            //Assert
            Assert.AreEqual("1", result);
        }

        [Test]
        public void GetUserId_InvalidUserName_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserId("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetUserById_ValidUserId_ReturnsUser()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserById("1");

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("test1", result.UserName);
        }

        [Test]
        public void GetUserById_InvalidUserId_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserById("100");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetUserByName_ValidUserName_ReturnsListOfUser()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserByName("test1");

            //Assert
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("test1", result[0].UserName);
        }

        [Test]
        public void GetUserByName_InvalidUserName_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserByName("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetUserByEmail_ValidUserEmail_ReturnsListOfUser()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserByEmail("test@test.com");

            //Assert
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("test1", result[0].UserName);
        }

        [Test]
        public void GetUserByEmail_InvalidUserEmail_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetUserByEmail("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void GetPasswordHash_ValidUserId_ReturnsHash()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetPasswordHash("1");

            //Assert
            Assert.AreEqual("passwordhash", result);
        }

        [Test]
        public void GetPasswordHash_InvalidUserId_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetPasswordHash("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void SetPasswordHash_ValidUserId_Returns1()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.SetPasswordHash("1", "newHash");

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual("newHash", db.users["1"].PasswordHash);
        }

        [Test]
        public void SetPasswordHash_InvalidUserId_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.SetPasswordHash("blah", "newHash");

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetSecurityStamp_ValidUserId_ReturnsValue()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetSecurityStamp("1");

            //Assert
            Assert.AreEqual("securitystamp", result);
        }

        [Test]
        public void GetSecurityStamp_InvalidUserId_ReturnsNull()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.GetSecurityStamp("blah");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void Insert_UserNotInTable_InsertsUser()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);

            //Act
            var result = sut.Insert(new ApplicationUser(){Id = "1"});

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(1, db.users.Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_UserInTable_ThrowsArgumentException()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Insert(new ApplicationUser() { Id = "1" });

            //Assert
            //Should throw exception
        }

        [Test]
        public void Update_UserInTable_UpdatesUser()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Update(new ApplicationUser() { Id = "1", PhoneNumber = "updated" });

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual("updated", db.users["1"].PhoneNumber);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_UserNotInTable_ThrowsArgumentException()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);

            //Act
            var result = sut.Update(new ApplicationUser() { Id = "1" });

            //Assert
            //Should throw exception
        }

        [Test]
        public void Delete_GivenValidUser_DeletesUserReturns1()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete(new ApplicationUser() { Id = "1" });

            //Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(1, db.users.Count());
        }

        [Test]
        public void Delete_GivenInValidUser_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete(new ApplicationUser() { Id = "100" });

            //Assert
            Assert.AreEqual(0, result);
            Assert.AreEqual(2, db.users.Count());
        }

        //
        // HELPERS
        //
        //
        private InMemoryContext getFullDb()
        {
            var db = new InMemoryContext();
            db.users.Add("1", new ApplicationUser()
            { 
                Id = "1",
                UserName =  "test1",
                Email = "test@test.com",
                AccessFailedCount = 0,
                EmailConfirmed = true,
                LockoutEnabled = true,
                LockoutEndDateUtc = DateTime.Now - TimeSpan.FromDays(1),
                PasswordHash = "passwordhash",
                PhoneNumber = "333-333-3333",
                PhoneNumberConfirmed = false,
                SecurityStamp = "securitystamp",
                TwoFactorEnabled = true               
            });

            db.users.Add("2", new ApplicationUser()
            {
                Id = "2",
                UserName = "test2",
                Email = "test2@test.com",
                AccessFailedCount = 1,
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEndDateUtc = DateTime.Now - TimeSpan.FromDays(1),
                PasswordHash = "testtest",
                PhoneNumber = "444-444-4444",
                PhoneNumberConfirmed = false,
                SecurityStamp = "securitystamptest",
                TwoFactorEnabled = true
            });

            return db;
        }

        private UserTable<ApplicationUser> getSut(InMemoryContext db)
        {
            return new UserTable<ApplicationUser>(db);
        }

    }
}
