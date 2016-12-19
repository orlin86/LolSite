using LolSite.Migrations;
using LolSite.Models;
using System.Data.Entity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LolSite.Startup))]
namespace LolSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SumonnerDbContext, Configuration>());


            ConfigureAuth(app);
        }
    }
}
