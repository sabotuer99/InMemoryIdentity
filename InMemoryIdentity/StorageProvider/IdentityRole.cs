﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IRole interface 
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
    }
}