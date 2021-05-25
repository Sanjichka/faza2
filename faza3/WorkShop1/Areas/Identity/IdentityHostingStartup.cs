using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkShop1.Areas.Identity.Data;
using WorkShop1.Data;

[assembly: HostingStartup(typeof(WorkShop1.Areas.Identity.IdentityHostingStartup))]
namespace WorkShop1.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WorkShop1Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WorkShop1Context")));

            });
        }
    }
}