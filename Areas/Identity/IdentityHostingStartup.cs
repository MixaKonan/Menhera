using System;
using Menhera.Areas.Identity.Data;
using Menhera.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Menhera.Areas.Identity.IdentityHostingStartup))]
namespace Menhera.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MenheraContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MenheraContextConnection")));

                services.AddDefaultIdentity<MenheraUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<MenheraContext>();
            });
        }
    }
}