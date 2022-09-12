using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using RepoDb;
using System.Collections;
using System.Web;
using RepoDb.DbSettings;
using RepoDb.DbHelpers;
using RepoDb.StatementBuilders;


namespace NineTails.DataAccess.DBAccess
{
    public class SQLAccess
    {

        private readonly IConfiguration _config;

        public SQLAccess(IConfiguration config)
        {
            _config = config;
        }

        public  string GetConnectionString(string name = "DapperDB")
        {
            return _config.GetConnectionString(name);
        }

        public SqlConnection CreateConnection(string connectionId = "Default")
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return connection;
        }

     
    }
}
