﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApp
{
    public class BooksDBContext:DbContext
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Books;Integrated Security=True";
        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var book = modelBuilder.Entity<Book>();
            //book.HasKey(p => p.BookId);
            //book.Property(p => p.Title).HasMaxLength(120).IsRequired();
            //book.Property(p => p.Publisher).HasMaxLength(50);
        }
    }
}
