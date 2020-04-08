﻿using System;
using IMMRequest.Domain;
using Microsoft.EntityFrameworkCore;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess
{
    public class IMMRequestContext : DbContext
    {
        public  DbSet<Request> Requests { get; set; }
		public  DbSet<Admin> Administrators { get; set; }
        public  DbSet<Area> Areas { get; set; }
        public  DbSet<Topic> Topics { get; set; }
        public  DbSet<Type> Types { get; set; }
        public  DbSet<AdditionalField> AdditionalFields { get; set; }

		public  IMMRequestContext(DbContextOptions  options) : base(options) { }
    }
}
