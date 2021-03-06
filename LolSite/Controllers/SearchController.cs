﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using LolSite.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace LolSite.Controllers
{
    public class SearchController : Controller
    {
        public string champName = "";
        public string server = "";


        //
        // GET: SearchPage
        public ActionResult SearchPage()
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            tempList.Add(new SelectListItem
            {
                Text = "EUNE",
                Value = "Europe North and East",
                Selected = true

            });
            tempList.Add(new SelectListItem
            {
                Text = "EUW",
                Value = "Western Europe",
            });
            tempList.Add(new SelectListItem
            {
                Text = "BR",
                Value = "Brasil"
            });
            tempList.Add(new SelectListItem
            {
                Text = "JP",
                Value = "Japan"
            });
            tempList.Add(new SelectListItem
            {
                Text = "KR",
                Value = "Korea"
            });
            tempList.Add(new SelectListItem
            {
                Text = "LAN",
                Value = "Latin America North "
            });
            tempList.Add(new SelectListItem
            {
                Text = "LAS",
                Value = "Latin America South"
            });
            tempList.Add(new SelectListItem
            {
                Text = "NA",
                Value = "North America"
            });
            tempList.Add(new SelectListItem
            {
                Text = "OCE",
                Value = "Oceania"
            });
            tempList.Add(new SelectListItem
            {
                Text = "RU",
                Value = "Russia"
            });
            tempList.Add(new SelectListItem
            {
                Text = "TR",
                Value = "Turkey"
            });

            ViewBag.Servers = tempList;
            return View();
        }

        //
        // POST: SearchPage
        [HttpPost]
        public ActionResult SearchResult(Summoner summoner)
        {
            server = summoner.Server;
            champName = summoner.SummonerName;
            //
            string output = "";
            string apiKey = "?api_key=RGAPI-83bc4cd9-c0d4-4fa3-a0a9-775ea5edc1e1";
            string Url =
                $"https://{server}.api.pvp.net/api/lol/{server}/v1.4/summoner/by-name/{champName}{apiKey}";

            bool hasRes = false;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Url);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(apiKey).Result;

            if (response.IsSuccessStatusCode)
            {
                hasRes = true;
            }

            if (hasRes)
            {
                JObject dataObjects = response.Content.ReadAsAsync<JObject>().Result;
                //string result = dataObjects.ToString(Newtonsoft.Json.Formatting.None);
                //Console.WriteLine(result);
                foreach (JProperty property in dataObjects.Properties())
                {
                    output += (property.Name + property.Value + System.Environment.NewLine);
                }

                if (output.Length > 0)
                {
                    string[] splitNameBody = output.Split('{').ToArray();
                    string username = splitNameBody[0];
                    string bodyTemp = splitNameBody[1];
                    string[] bodyArgs = bodyTemp
                        .Split(new[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(s => s.Trim())
                        .Select(s => s.Trim('"', '}'))
                        .ToArray();



                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    for (int i = 0; i < bodyArgs.Length - 1; i += 2)
                    {
                        dic.Add(bodyArgs[i], bodyArgs[i + 1]);
                    }

                    // MillisecondsToDate
                    foreach (var kvp in dic)
                    {
                        if (kvp.Key == "revisionDate")
                        {
                            long timeInMs = long.Parse(kvp.Value);
                            DateTime revDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            revDate = revDate.AddMilliseconds(timeInMs);
                            revDate = revDate.ToLocalTime();
                            dic[kvp.Key] = revDate.ToString(@"dd\/MM\/yyyy HH:mm");
                            break;
                        }
                    }

                    // UrlForImg
                    string imgUrl = "http://ddragon.leagueoflegends.com/cdn/6.5.1/img/profileicon/";
                    int imgNum = 0;
                    foreach (var kvp in dic)
                    {
                        if (kvp.Key == "profileIconId")
                        {
                            imgUrl += kvp.Value + ".png";
                            imgNum += int.Parse(kvp.Value);
                        }
                    }

                    long sumonID = 0;
                    foreach (var kvp in dic)
                    {
                        if (kvp.Key == "id")
                        {
                            sumonID += long.Parse(kvp.Value);
                        }
                    }

                    using (var database = new SumonnerDbContext())
                    {
                        // Get ownerId
                        if (User.Identity.IsAuthenticated)
                        {
                            var ownerId = database.Users
                            .Where(u => u.UserName == this.User.Identity.Name)
                            .First()
                            .Id;
                            bool summExist = database.Summoners.Any(t => t.SummonerID == sumonID && t.OwnerId == ownerId);
                            ViewBag.sumEx = summExist;
                        }
                        
                    }


                    // Output
                    summoner.ProfileIconID = imgNum;
                    summoner.SummonerID = sumonID;

                    ViewBag.ImgSrc = imgUrl;
                    ViewBag.Username = username + ":";
                    ViewBag.Data = dic;

                }
                return View(summoner);
            }
            else
            {
                return Redirect("/Search/NoSummonerFound");
            }
        }

        //
        // GET: SearchResult
        public ActionResult SummonerDetails(Summoner summoner)
        {
            string imgUrl = "http://ddragon.leagueoflegends.com/cdn/6.5.1/img/profileicon/";

            imgUrl += summoner.ProfileIconID.ToString() + ".png";
            

            ViewBag.ImgSrc = imgUrl;
            ViewBag.Data = summoner;
            return View(summoner);
        }

        //
        // POST: SearchResult
        [HttpPost]
        public ActionResult AddSummoner(Summoner summoner)
        {

            if (ModelState.IsValid)
            {
                using (var database = new SumonnerDbContext())
                {
                    // Get ownerId
                    var ownerId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    // Set ownerId
                    summoner.OwnerId = ownerId;

                    database.Summoners.Add(summoner);
                    database.SaveChanges();

                    return Redirect("/Search/MySummoners");

                }
            }
            return View(summoner);
        }

        //
        // POST: MySummoners
        public ActionResult DeleteSummoner(Summoner summoner)
        {

            using (var database = new SumonnerDbContext())
            {
                var summonerToDel = database.Summoners
                    .Where(u => u.SummonerName == summoner.SummonerName)
                    .First();

                database.Summoners.Remove(summonerToDel);
                database.SaveChanges();

                return Redirect("/Search/MySummoners");
            }

        }

        //
        // GET: NoSummonerFound
        public ActionResult NoSummonerFound()
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            tempList.Add(new SelectListItem
            {
                Text = "EUNE",
                Value = "Europe North and East",
                Selected = true

            });
            tempList.Add(new SelectListItem
            {
                Text = "EUW",
                Value = "Western Europe",
            });
            tempList.Add(new SelectListItem
            {
                Text = "BR",
                Value = "Brasil"
            });
            tempList.Add(new SelectListItem
            {
                Text = "JP",
                Value = "Japan"
            });
            tempList.Add(new SelectListItem
            {
                Text = "KR",
                Value = "Korea"
            });
            tempList.Add(new SelectListItem
            {
                Text = "LAN",
                Value = "Latin America North "
            });
            tempList.Add(new SelectListItem
            {
                Text = "LAS",
                Value = "Latin America South"
            });
            tempList.Add(new SelectListItem
            {
                Text = "NA",
                Value = "North America"
            });
            tempList.Add(new SelectListItem
            {
                Text = "OCE",
                Value = "Oceania"
            });
            tempList.Add(new SelectListItem
            {
                Text = "RU",
                Value = "Russia"
            });
            tempList.Add(new SelectListItem
            {
                Text = "TR",
                Value = "Turkey"
            });

            ViewBag.Servers = tempList;
            return View();
        }

        // GET: MySummoners
        public ActionResult MySummoners()
        {
            using (var database = new SumonnerDbContext())
            {
                //Get this.User Id
                var userId = database.Users
                    .Where(u => u.UserName == this.User.Identity.Name)
                    .First().Id;

                //Get saved summoners from the database
                var summoners = database.Summoners
                    .Where(u => u.OwnerId == userId)
                    .ToList();

                return View(summoners);
            }
        }


    }
}
