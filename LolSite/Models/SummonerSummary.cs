using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LolSite.Models
{
    public class SummonerSummary
    {
        public Playerstatsummary[] playerStatSummaries { get; set; }
        public int summonerId { get; set; }

        public class Playerstatsummary
        {
            public string playerStatSummaryType { get; set; }
            public Aggregatedstats aggregatedStats { get; set; }
            public long modifyDate { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }
        }

        public class Aggregatedstats
        {
            public int totalChampionKills { get; set; }
            public int totalAssists { get; set; }
            public int totalTurretsKilled { get; set; }
            public int totalNeutralMinionsKilled { get; set; }
            public int totalMinionKills { get; set; }
            public int averageNodeCaptureAssist { get; set; }
            public int maxNodeNeutralizeAssist { get; set; }
            public int maxChampionsKilled { get; set; }
            public int averageChampionsKilled { get; set; }
            public int averageNumDeaths { get; set; }
            public int maxNodeCapture { get; set; }
            public int maxObjectivePlayerScore { get; set; }
            public int maxAssists { get; set; }
            public int averageCombatPlayerScore { get; set; }
            public int maxNodeCaptureAssist { get; set; }
            public int averageObjectivePlayerScore { get; set; }
            public int maxTeamObjective { get; set; }
            public int averageNodeCapture { get; set; }
            public int averageTotalPlayerScore { get; set; }
            public int averageTeamObjective { get; set; }
            public int averageNodeNeutralize { get; set; }
            public int maxNodeNeutralize { get; set; }
            public int averageNodeNeutralizeAssist { get; set; }
            public int averageAssists { get; set; }
            public int maxTotalPlayerScore { get; set; }
            public int maxCombatPlayerScore { get; set; }
            public int totalNodeNeutralize { get; set; }
            public int totalNodeCapture { get; set; }
        }
    }
}