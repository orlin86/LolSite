using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LolSite.Models
{
    public class SummonerRankedData
    {

        public long modifyDate { get; set; }
        public Champion[] champions { get; set; }

        [Key]
        public int summonerId { get; set; }

        public class Champion
        {
            public int id { get; set; }
            public string champName { get; set; }
            public Stats stats { get; set; }
        }

        public class Stats
        {
            public int TotalDeathsPerSession { get; set; }
            public int TotalSessionsPlayed { get; set; }
            public int TotalDamageTaken { get; set; }
            public int TotalQuadraKills { get; set; }
            public int TotalTripleKills { get; set; }
            public int TotalMinionKills { get; set; }
            public int MaxChampionsKilled { get; set; }
            public int TotalDoubleKills { get; set; }
            public int TotalPhysicalDamageDealt { get; set; }
            public int TotalChampionKills { get; set; }
            public int TotalAssists { get; set; }
            public int MostChampionKillsPerSession { get; set; }
            public int TotalDamageDealt { get; set; }
            public int TotalFirstBlood { get; set; }
            public int TotalSessionsLost { get; set; }
            public int TotalSessionsWon { get; set; }
            public int TotalMagicDamageDealt { get; set; }
            public int TotalGoldEarned { get; set; }
            public int TotalPentaKills { get; set; }
            public int TotalTurretsKilled { get; set; }
            public int MostSpellsCast { get; set; }
            public int MaxNumDeaths { get; set; }
            public int TotalUnrealKills { get; set; }
            public int MaxLargestCriticalStrike { get; set; }
            public int RankedPremadeGamesPlayed { get; set; }
            public int TotalNeutralMinionsKilled { get; set; }
            public int MaxLargestKillingSpree { get; set; }
            public int MaxTimeSpentLiving { get; set; }
            public int MaxTimePlayed { get; set; }
            public int BotGamesPlayed { get; set; }
            public int KillingSpree { get; set; }
            public int RankedSoloGamesPlayed { get; set; }
            public int TotalHeal { get; set; }
            public int NormalGamesPlayed { get; set; }
        }


    }
}