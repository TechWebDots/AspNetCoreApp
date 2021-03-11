using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {                    
                    await context.Response.WriteAsync("Practical Session on EF Core byTechWebDots");
                    AddBookAsync("Book1", "publisher", context).Wait();
                });
            });
        }
        public async Task AddBookAsync(string title, string publisher, HttpContext httpContext)
        {
            using (var context = new BooksDBContext())
            {
                var book = new Book { Title = title, Publisher = publisher };
                context.Add(book);
                int records = await context.SaveChangesAsync();

                await httpContext.Response.WriteAsync($"{records} record added");

            }
            await httpContext.Response.WriteAsync("");
        }

    }
}
