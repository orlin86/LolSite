using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LolSite.Models
{
    public class SumonnerDbContext : IdentityDbContext<ApplicationUser>
    {
        public SumonnerDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        

        public virtual IDbSet<Summoner> Summoners { get; set; }

        public virtual IDbSet<SummonerRankedData> SummonersRankedDatas { get; set; }

        public static SumonnerDbContext Create()
        {
            return new SumonnerDbContext();
        }
    }
}