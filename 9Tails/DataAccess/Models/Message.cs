using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace NineTails.DataAccess.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        
        public string UserName { get; set; }

        public string Avatar { get; set; }


        public string Text { get; set; }

        public DateTime Time { get; set; } = DateTime.UtcNow;

    }
}
