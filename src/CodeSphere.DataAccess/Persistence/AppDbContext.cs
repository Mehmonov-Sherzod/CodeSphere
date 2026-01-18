using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.DataAccess.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Course> Courses { get; set; }  
        public DbSet<Topic> Topic { get; set; }
        public DbSet<DsaQuestions> DsaQuestions { get; set; }
        public DbSet<DsaTopics> DsaTopics { get; set; }
        public DbSet<DsaQuestionTestCases> DsaQuestionTestCases { get; set; }

        public DbSet<DsaTopicQuestions> DsaTopicQuestions { get; set; } 
        public DbSet<Videos> Videos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<UserRole>()
               .HasKey(rp => new { rp.RoleId, rp.UserId });
        }
    }
}
