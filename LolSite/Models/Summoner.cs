using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LolSite.Models
{
    public class Summoner
    {
        [StringLength(50)]
        public string SummonerName { get; set; }

        public string Server { get; set; }

        public long SummonerID { get; set; }

        public int ProfileIconID { get; set; }
    }
}



