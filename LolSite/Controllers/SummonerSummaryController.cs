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

namespace LolSite.Controllers
{
    public class SummonerSummaryController : Controller
    {

        // GET: Summary
        public ActionResult Summary(Summoner summoner)
        {
            {

        bool hasRes = false;
                HttpClient client = new HttpClient();

                string uri = $"https://{summoner.Server}.api.pvp.net/api/lol/{summoner.Server}/v1.3/stats/by-summoner/{summoner.SummonerID}/summary?season=SEASON2016&api_key=RGAPI-83bc4cd9-c0d4-4fa3-a0a9-775ea5edc1e1";

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
                    SummonerSummary summary = new SummonerSummary();
                    string data = response.Content.ReadAsAsync<JObject>().Result.ToString();
                    summary = JsonConvert.DeserializeObject<SummonerSummary>(data);

                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    ViewBag.Data = summary.playerStatSummaries;
                    return View(summary);
                }
                else
                {
                    return Redirect("/SummonerRankedData/NoRankedStats");
                }

            }
        }
    }
}