using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string? Broadcaster { get; set; }

        public string Status { get; set; } = "disabled";

        public bool Active { get; set; } = false;

        public List<PlaylistItem> Playlist { get; set; } = new List<PlaylistItem> { };
        public List<string> Members { get; set; } = new List<string> { };
        public List<Message> Messages { get; set; } = new List<Message> { };


       
        


    }
}
