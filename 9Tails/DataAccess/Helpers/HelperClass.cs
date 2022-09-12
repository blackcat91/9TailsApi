using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RepoDb;
using RepoDb.DbSettings;
using RepoDb.DbHelpers;
using RepoDb.StatementBuilders;
using AngleSharp;
using AngleSharp.Js;

using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;
using NineTails.DataAccess.DBAccess;
using NineTails.DataAccess.Models;

namespace NineTails.DataAccess.Helpers
{
    public class HelperClass
    {

        private readonly AngleSharp.IConfiguration? _config;
       
        private readonly HttpClient _http;
        private readonly IBrowsingContext? _context;
        private readonly SqlServerDbSetting? _dbSettings;
        private readonly SQLAccess _sql;

        public HelperClass( SQLAccess sql)
        {
            _http = new HttpClient();
            
            _config = Configuration.Default.WithJs().WithDefaultLoader();
            _context = BrowsingContext.New(_config);
            SqlServerBootstrap.Initialize();
            _dbSettings = new SqlServerDbSetting();
            DbSettingMapper
                .Add<SqlConnection>(_dbSettings, true);
            DbHelperMapper
                .Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<SqlConnection>(new SqlServerStatementBuilder(_dbSettings), true);
            _sql = sql;
        }

        public async Task<string?> CheckLink(PlaylistItem item)
        {
            var response = await _http.GetStringAsync(item.Url);
            var document = await _context!.OpenAsync(r => r.Content(response)).WaitAsync(TimeSpan.FromMinutes(5));

            var videoScript = document.GetElementsByTagName("script")[1].InnerHtml;

            var link = videoScript.Split("\"")[3];

          
            
            if (link.Contains(".m3u8"))
            {
                
                return link;
            }
            else if(link.Contains(".mp4"))
            {
                return await PrepareLink(link, item.SeriesId, item.Episode);
            }
            else
            {
                return null;
            }
            
        }

        public async Task<string> PrepareLink(string link, int anime, int episode)
        {
            var endOfLink = link.Split('/')[link.Split('/').Length - 1];
            using (SqlConnection connection = _sql.CreateConnection())
            {
                var exists = await connection.QueryAsync<Links>(l => l.Url.Contains(endOfLink));
                if (exists.Any()) return exists.First().Url;
                else return await UploadUrl(link, anime, episode);



            }


        }

        public async Task<string> UploadUrl(string link, int anime, int episode)
        {
            

            var url = new VideoUrl{ Url = link, Anime = anime, Episode = episode };



            var response = await _http.PostAsJsonAsync("https://us-central1-animates-361800.cloudfunctions.net/downloadVideo", url);
           
            return await response.Content.ReadAsStringAsync();
        }
    }

   
public class VideoUrl
    {
        public string? Url { get; set; }
        public int? Anime { get; set; }
        public int? Episode { get; set; }
    }
}
