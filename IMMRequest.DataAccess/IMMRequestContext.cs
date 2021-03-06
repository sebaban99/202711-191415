﻿using IMMRequest.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess
{
    [ExcludeFromCodeCoverage]
    public class IMMRequestContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Admin> Administrators { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<AdditionalField> AdditionalFields { get; set; }
        public DbSet<AFRangeItem> RangeValues { get; set; }
        public DbSet<AFValueItem> AFValueItems { get; set; }
        public DbSet<AFValue> AFValues { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Log> Logs { get; set; }
        public object Session { get; internal set; }

        public IMMRequestContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
               .HasMany(t => t.Topics)
               .WithOne(t => t.Area);
            modelBuilder.Entity<Topic>()
               .HasMany(ty => ty.Types)
               .WithOne(tp => tp.Topic);
            modelBuilder.Entity<Type>()
               .HasMany(ad => ad.AdditionalFields)
               .WithOne(tp => tp.Type);
            modelBuilder.Entity<AdditionalField>()
               .HasKey(a => a.Id);
            modelBuilder.Entity<AdditionalField>()
               .HasMany(r => r.Range)
               .WithOne(a => a.AdditionalField);
            modelBuilder.Entity<Request>()
               .HasOne(t => t.Type);
            modelBuilder.Entity<Request>()
              .HasMany(afv => afv.AddFieldValues)
              .WithOne(r => r.Request);
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();
            modelBuilder.Entity<AFValue>()
                .HasOne(a => a.AdditionalField);
        }
    }
}
