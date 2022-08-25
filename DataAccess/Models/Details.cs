using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Details
    {

		public int Id { get; set; }
		public string Title { get; set; }
		public string Poster { get; set; }
		public string Overview { get; set; }
		public string OtherNames { get; set; }
		public string Language { get; set; }
		public string Episodes { get; set; }
		public string Views { get; set; }
		public DateTime LastAdded { get; set; }
		public int Release { get; set; }
		public string Type { get; set; }
		public string Status { get; set; }
		

    }
}
