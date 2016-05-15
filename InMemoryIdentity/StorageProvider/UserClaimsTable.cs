using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    /// <summary>
    /// Class that represents the UserClaims table in the MySQL Database
    /// </summary>
    public class UserClaimsTable
    {
        
        private InMemoryContext _database;

        /// <summary>
        /// Constructor that takes a InMemoryContext instance 
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable(InMemoryContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            if (!_database.claims.ContainsKey(userId))
                return new ClaimsIdentity();

            var claims = _database.claims[userId];

            return new ClaimsIdentity(claims);
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            if (!_database.claims.ContainsKey(userId))
                return 0;

            var deleted = _database.claims[userId].Count;
            _database.claims.Remove(userId);
            return deleted;
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            if (!_database.claims.ContainsKey(userId))
                _database.claims[userId] = new List<Claim>();

            if (_database.claims[userId].Where(x => 
                x.Type == userClaim.Type && 
                x.Value == userClaim.Value).Count() > 0)
                throw new ArgumentException("Insert failed: Duplicate claim.");

            _database.claims[userId].Add(userClaim);

            return 1;
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            if (!_database.claims.ContainsKey(user.Id))
                return 0;

            var claims = _database.claims[user.Id];
            var remove = claims.Where(x => x.Type == claim.Type && x.Value == claim.Value).FirstOrDefault();

            return claims.Remove(remove) ? 1 : 0;
        }
    }
 
}