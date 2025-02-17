﻿using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Class> Classess { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<UnassignedLesson> UnassignedLessons { get; set;}

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderRelations.InitializeRelations(modelBuilder);

            
            modelBuilder.Entity<Classroom>()
                .HasIndex(x => new { x.TimetableId, x.Code }).IsUnique();

            modelBuilder.Entity<Class>()
                .HasIndex(x => new { x.TimetableId, x.Name }).IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(x => new { x.TimetableId, x.Name, x.ClassId }).IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasIndex(x => new { x.TimetableId, x.FirstName, x.LastName }).IsUnique();

            modelBuilder.Entity<Group>()
                .HasIndex(x => new { x.TimetableId, x.Name }).IsUnique();

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entries)
            {
                ((AuditableEntity)entity.Entity).LastModified = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((AuditableEntity)entity.Entity).Created = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}