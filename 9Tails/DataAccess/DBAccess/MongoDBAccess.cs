using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;


namespace NineTails.DataAccess.DBAccess
{
    public class MongoDBAccess
    {
        
        private readonly IConfiguration _configuration;

        public MongoDBAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public const string DatabaseName = "Animates";
     

        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }


       

        /*

        public Jwt? VerifyJwt()
        {
            try
            {
                var file = System.IO.File.ReadAllText(Path.GetTempPath() + "token.json");
                Jwt? jwt = JsonConvert.DeserializeObject<Jwt>(file);
                
                if (jwt!.Expiry > DateTime.UtcNow)
                {
                   
                    return jwt;
                }
                else
                {
                    File.Delete(Path.GetTempPath() + "token.json");
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        */

       
    }
}
