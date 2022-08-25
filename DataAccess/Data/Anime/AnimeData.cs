using DataAccess.DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;
using System.Collections;
using System.Web;
using RepoDb.DbSettings;
using RepoDb.DbHelpers;
using RepoDb.StatementBuilders;
using System.Data.SqlClient;
using RepoDb.Enumerations;
using DataAccess.Models;

namespace DataAccess.Data.Anime
{
    public class AnimeData : IAnimeData
    {
        private readonly SqlServerDbSetting? _dbSettings;
        private readonly SQLAccess _sql;

        public AnimeData(SQLAccess sql)
        {
            _sql = sql;
            SqlServerBootstrap.Initialize();
            _dbSettings = new SqlServerDbSetting();
            DbSettingMapper
                .Add<SqlConnection>(_dbSettings, true);
            DbHelperMapper
                .Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<SqlConnection>(new SqlServerStatementBuilder(_dbSettings), true);
        }

        public async Task<IEnumerable<Details>?> SearchAnimes(string query)
        {
            query = $"{query}%";
            var queryFields = new[]
            {
                new QueryField("Title", Operation.Like, query),
                new QueryField("OtherNames", Operation.Like, query),
            };
            var queryGroup = new QueryGroup(queryFields, Conjunction.Or);


            using (var conn = _sql.CreateConnection())
            {
                try
                {
                    var results = (await conn.QueryAsync<Details>(queryGroup)).OrderBy(d => d.Title);
                    
                    return results;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }

        }

        public async Task<IEnumerable<Links>?> GetLinks(int id)
        {
            using (var conn = _sql.CreateConnection())
            {
                try
                {
                    var orderBy = new OrderField("Episode", Order.Ascending);
                    var results = (await conn.QueryAsync<Links>(where: new QueryField("SeriesId", Operation.Equal, id), orderBy: new List<OrderField>{ orderBy }));;
                    return results;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }
    }
}
