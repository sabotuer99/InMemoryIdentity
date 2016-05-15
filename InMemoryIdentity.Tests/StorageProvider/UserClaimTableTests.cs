using InMemoryIdentity.StorageProvider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryIdentity.Tests.StorageProvider
{
    [TestFixture]
    class UserClaimTableTests
    {
        [Test]
        public void Insert_UserHasNoClaims_AddsClaimToUser()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);

            //Act
            var result = sut.Insert(new Claim("test","test"), "1");

            //Assert
            Assert.AreEqual(1, db.claims.Count);
            Assert.True(db.claims.ContainsKey("1"));
        }

        [Test]
        public void Insert_UserHasSomeClaims_AddsNewClaimToUser()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            db.claims["1"] = new List<Claim>() { new Claim("existing", "existing")};

            //Act
            var result = sut.Insert(new Claim("test", "test"), "1");

            //Assert
            Assert.AreEqual(2, db.claims["1"].Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_UserHasClaimAlready_ThrowsArgumentException()
        {
            //Arrange
            var db = new InMemoryContext();
            var sut = getSut(db);
            db.claims["1"] = new List<Claim>() { new Claim("test", "test") };

            //Act
            var result = sut.Insert(new Claim("test", "test"), "1");

            //Assert
            //Should throw an exception
        }

        [Test]
        public void Delete_GivenUserIdThatExists_DeletesAllClaimsReturnsCount()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("1");

            //Assert
            Assert.AreEqual(1, db.claims.Count);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Delete_GivenUserIdThatDoesNotExist_Returns0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete("10");

            //Assert
            Assert.AreEqual(2, db.claims.Count);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Delete_GivenUserAndClaimThatExist_DeletesClaim()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete(new IdentityUser() { Id = "1" }, new Claim("one", "one"));

            //Assert
            Assert.AreEqual(2, db.claims.Count);
            Assert.AreEqual(2, db.claims["1"].Count);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Delete_GivenUserExistsAndClaimDoesNot_Return0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete(new IdentityUser() { Id = "1" }, new Claim("duh", "duh"));

            //Assert
            Assert.AreEqual(2, db.claims.Count);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Delete_GivenUserDoesNotExist_Return0()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.Delete(new IdentityUser() { Id = "10" }, new Claim("duh", "duh"));

            //Assert
            Assert.AreEqual(2, db.claims.Count);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void FindByUserId_UserExists_ReturnClaimsIdentityWithAllClaims()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.FindByUserId("1");

            //Assert
            Assert.False(result.IsAuthenticated);
            Assert.AreEqual(3, result.Claims.Count());
            Assert.AreEqual(1, result.Claims.Where(x => x.Value == "one").Count());
        }

        [Test]
        public void FindByUserId_UserDoesntExist_ReturnNewClaimsIdentity()
        {
            //Arrange
            var db = getFullDb();
            var sut = getSut(db);

            //Act
            var result = sut.FindByUserId("10");

            //Assert
            Assert.False(result.IsAuthenticated);
            Assert.AreEqual(0, result.Claims.Count());
        }


        private InMemoryContext getFullDb()
        {
            var db = new InMemoryContext();
            db.claims["1"] = new List<Claim>() { new Claim("one", "one") };
            db.claims["1"].Add(new Claim("two", "two"));
            db.claims["1"].Add(new Claim("three", "three"));

            db.claims["2"] = new List<Claim>() { new Claim("one", "one") };
            db.claims["2"].Add(new Claim("two", "two"));
            db.claims["2"].Add(new Claim("three", "three"));

            return db;
        }

        private UserClaimsTable getSut(InMemoryContext db)
        {
            return new UserClaimsTable(db);
        }

    }
}
