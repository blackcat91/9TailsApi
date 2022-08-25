using AngleSharp;
using AngleSharp.Js;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using DataAccess.DBAccess;
using Scraper.Models;

using System.Collections;
using System.Web;
using RepoDb;
using RepoDb.DbSettings;
using RepoDb.DbHelpers;
using RepoDb.StatementBuilders;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Io;

namespace Scraper
{
    public class Program
    {



        static async Task Main(string[] args)
        {
            Console.WriteLine("Trying...");
            try
            {
                await GenerateMethods();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed: {e.Message}");
            }
            
            



        }

         static async Task GenerateMethods()
        {
           

            for(int i=79; i<=392; i+=3)
            {
                var scraper = new DataScraper();
                var tasks = new List<Task>();
                tasks.Add(Task.Run(() => scraper.Initialize(i)));
                tasks.Add(Task.Run(() => scraper.Initialize(i+1)));
                tasks.Add(Task.Run(() => scraper.Initialize(i+2)));
                
                await Task.WhenAll(tasks);

            }
        }


    }

    public class DataScraper
    {
        private readonly IConfiguration? _config;
        private readonly HttpClient _http;
        private readonly IBrowsingContext? _context;
        private readonly SqlServerDbSetting? _dbSettings;
        string BASEURL = "https://animefrenzy.net";
        string PAGEURL = "https://animefrenzy.net/anime?sort_alphabet=a-z&page=";
        string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=9TailsSQL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DataScraper()
        {
            _http = new HttpClient();
            _config = AngleSharp.Configuration.Default.WithJs().WithDefaultLoader();
            _context = BrowsingContext.New(_config);
            SqlServerBootstrap.Initialize();
            _dbSettings = new SqlServerDbSetting();
            DbSettingMapper
                .Add<System.Data.SqlClient.SqlConnection>(_dbSettings, true);
            DbHelperMapper
                .Add<System.Data.SqlClient.SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<System.Data.SqlClient.SqlConnection>(new SqlServerStatementBuilder(_dbSettings), true);


        }


        public async Task Initialize(int page)
        {
            var response = await _http.GetAsync(string.Concat(PAGEURL, page));
            if (((int)response.StatusCode) == 200)
            {
                var document = await _context!.OpenAsync(r => r.Content(response.Content.ReadAsStream())).WaitAsync(TimeSpan.FromMinutes(5));

                var animes = document.QuerySelectorAll(".flw-item > .film-poster > a");
                foreach (var anime in animes)
                {
                    try
                    {
                        var animeHTML = await _http.GetStringAsync(string.Concat(BASEURL, anime.GetAttribute("href")));
                        var animePage = await _context!.OpenAsync(r => r.Content(animeHTML)).WaitAsync(TimeSpan.FromMinutes(5));
                        Console.WriteLine($"----- {anime.GetAttribute("href")}");
                        string? title;
                        title = HttpUtility.HtmlEncode(animePage.QuerySelector(".film-name")!.TextContent.Trim());


                        Console.WriteLine($"Starting {title} Episodes!");
                        var episodesHTML = await _http.GetStringAsync(string.Concat(BASEURL, animePage.QuerySelector(".film-buttons .btn-play")!.GetAttribute("href")));
                        var episodesLink = await _context!.OpenAsync(r => r.Content(episodesHTML)).WaitAsync(TimeSpan.FromMinutes(5));
                        var episodes = episodesLink.QuerySelectorAll(".detail-infor-content>.ss-list> a");
                        int? animeId;
                        Console.WriteLine();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            animeId = connection.Query<Details>(p => p.Title == title).FirstOrDefault()!.Id;
                        }
                        await GetEpisodes(episodes, animeId);
                        Console.WriteLine($"{title} Episodes Complete!");
                    }
                    catch (Exception e)
                    {
                        await GetAnime(anime);
                    }
                }


            }


        }
        public async Task GetAnime(AngleSharp.Dom.IElement? anime)
        {
            try
            {
                var animeHTML = await _http.GetStringAsync(string.Concat(BASEURL, anime.GetAttribute("href")));
                var animePage = await _context!.OpenAsync(r => r.Content(animeHTML)).WaitAsync(TimeSpan.FromMinutes(5));
                Console.WriteLine($"----- {anime.GetAttribute("href")}");
                string? title;
                title = HttpUtility.HtmlEncode(animePage.QuerySelector(".film-name")!.TextContent.Trim());


                Console.WriteLine($"Starting {title} Episodes!");
                var episodesHTML = await _http.GetStringAsync(string.Concat(BASEURL, animePage.QuerySelector(".film-buttons .btn-play")!.GetAttribute("href")));
                var episodesLink = await _context!.OpenAsync(r => r.Content(episodesHTML));
                var episodes = episodesLink.QuerySelectorAll(".detail-infor-content>.ss-list> a");
                int? animeId;
                Console.WriteLine();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    animeId = connection.Query<Details>(p => p.Title == title).FirstOrDefault()!.Id;
                }
                await GetEpisodes(episodes, animeId);
                Console.WriteLine($"{title} Episodes Complete!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task GetEpisodes(AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> episodes, int? animeId)
        {
            Console.WriteLine("Creating Data...");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var episode in episodes)
                {
                    var document = await _context!.OpenAsync(string.Concat("https://animefrenzy.net", episode.GetAttribute("href"))).WaitAsync(TimeSpan.FromMinutes(5));

                    var links = document.QuerySelectorAll("a.btn-server");
                    

                    var href = links[0].GetAttribute("href");
                    if (href!.StartsWith("/"))
                    {
                        href = string.Concat("https://animefrenzy.net", href);
                    }
                    var data = new Links();
                    data.SeriesId = animeId;
                    data.Source = links[0].TextContent.Trim();
                    var epNum = episode.TextContent.Trim();
                    try { data.Episode = Int32.Parse(epNum); }
                    catch { data.Episode = Int32.Parse(epNum.Split('-')[0]); }
                    var exists = connection.Query<Links>(d => d.SeriesId == data.SeriesId && d.Episode == data.Episode);
                    Console.WriteLine("Checking For Duplicates...");
                    if (exists.Any()) continue;
                    data.Link = href;
                    connection.Insert<Links>(data);
                    

                }
                Console.WriteLine("Inserts Completed \n");
            }

        }


    }
}


