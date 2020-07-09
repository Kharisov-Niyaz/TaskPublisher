using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskPublisher
{
    class TaskDbContext : DbContext
    {
        public DbSet<Packet> packets { get; set; }
        public TaskDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server =.\SQLEXPRESS02; Database = taskDbPublisher; Trusted_Connection = True;");
            optionsBuilder.UseSqlite($"Data Source={Program.dbName}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Packet>().HasData(
                new Packet[]
                {
                    new Packet() { id = 1, message = "message 1", sended = false },
                    new Packet() { id = 2, message = "message 2", sended = false },
                    new Packet() { id = 3, message = "message 3", sended = false },
                    new Packet() { id = 4, message = "message 4", sended = false }
                }
                );
        }
    }
}
