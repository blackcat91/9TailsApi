using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class Links
    {
  
    public int Id { get; set; }
    public int? SeriesId { get; set; }
	public int? Episode { get; set; }
	public string Source { get; set; }
	public string Link { get; set; }

       
    }
}
