using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreApp.DataAccess;
using AspNetCoreApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContext<BookContext>(options => options.UseSqlServer("name=ConnectionStrings:BookContext"));
            services.AddTransient<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Books}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("<h1>Practical Session on MVC setup in .NET Core by TechWebDots</h1>");
            //});

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Practical Session on EF Core by TechWebDots\n");
            //        CreateDatabaseAsync(context).Wait();
            //        AddBookAsync("Book1", "publisher", context).Wait();
            //    });
            //});
        }

        //public static async Task CreateDatabaseAsync(HttpContext httpContext)
        //{
        //    using (var context = new BookContext())
        //    {
        //        bool created = await context.Database.EnsureCreatedAsync();

        //        string createdText = created ? "created\n" : "already exists\n";
        //        await httpContext.Response.WriteAsync($"database {createdText}");
        //    }
        //}
        //public async Task AddBookAsync(string title, string publisher, HttpContext httpContext)
        //{
        //    using (var context = new BookContext())
        //    {
        //        var book = new Book { Title = title, Publisher = publisher };
        //        context.Add(book);
        //        int records = await context.SaveChangesAsync();

        //        await httpContext.Response.WriteAsync($"{records} record added successfully! \n");

        //    }
        //    await httpContext.Response.WriteAsync("");
        //}

    }
}
