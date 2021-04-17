using System;
using AspNetCoreApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApp.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(BookContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Books.Any())
            {
                return;   // DB has been seeded
            }

            var books = new Book[]
            {
            new Book{Publisher="Publisher 1",Title="Title 1"},
            new Book{Publisher="Publisher 2",Title="Title 2"},
            new Book{Publisher="Publisher 3",Title="Title 3"}

            };
            foreach (Book s in books)
            {
                context.Books.Add(s);
            }
            context.SaveChanges();
        }
    }
}
