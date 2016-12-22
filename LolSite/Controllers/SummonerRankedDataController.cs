using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using LolSite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using LolSite;


namespace LolSite.Controllers
{
    public class SummonerRankedDataController : Controller
    {
        // GET: SummonerRankedData
        public ActionResult RankedStats(Summoner summoner)
        {
            bool hasRes = false;
            HttpClient client = new HttpClient();

            string uri =
            $"https://{summoner.Server}.api.pvp.net/api/lol/{summoner.Server}/v1.3/stats/by-summoner/{summoner.SummonerID}/ranked?season=SEASON2016&api_key=RGAPI-83bc4cd9-c0d4-4fa3-a0a9-775ea5edc1e1";
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                hasRes = true;
            }
            if (hasRes)
            {
                SummonerRankedData root = new SummonerRankedData();
                string data = response.Content.ReadAsAsync<JObject>().Result.ToString();
                root = JsonConvert.DeserializeObject<SummonerRankedData>(data);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                
                string[] tempStringArr = System.IO.File.ReadAllLines(Server.MapPath("~\\ChampIdsToNames.txt"));
                foreach (var arg in tempStringArr)
                {
                    string[] temp = arg.Split(' ');
                    dic[temp[0]] = temp[1];
                }

                foreach (var kvp in dic)
                {
                    foreach (var champ in root.champions)
                    {
                        if (champ.id.ToString()==kvp.Key)
                        {
                            champ.champName = kvp.Value;
                            champ.url = $"http://ddragon.leagueoflegends.com/cdn/6.24.1/img/champion/{kvp.Value}.png";
                        }
                    }
                }
                
                ViewBag.Data = root.champions.OrderByDescending(a => a.id);
                return View(root);
            }
            else
            {
                return Redirect("/SummonerRankedData/NoRankedStats");
            }

        }
     
        // Get: NoRankedStats
        public ActionResult NoRankedStats()
        {
            return View();
        }
    }
}