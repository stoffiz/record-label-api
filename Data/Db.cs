using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecordLabelApi.Data;

namespace RecordLabelApi.Data
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Release>()
                .Property(r => r.ReleaseDate)
                .IsRequired(false);

            modelBuilder.Entity<Message>()
                .Property(m => m.IsRead)
                .HasDefaultValue(false);

            modelBuilder.Entity<Message>()
                .Property(m => m.IsAnswered)
                .HasDefaultValue(false);

        }

        

        public DbSet<Release> Releases { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
