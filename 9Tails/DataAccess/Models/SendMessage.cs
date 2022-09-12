using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NineTails.DataAccess.Models
{
    public class SendMessage
    {
    
        public string Name { get; set; }
     
        public Message Message { get; set; }

       
        


    }
}
