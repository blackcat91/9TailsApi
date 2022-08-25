using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class Episodes
    {
        int Id { get; set; }

    int SeriesId { get; set; }
	int Episode { get; set; }
	string Download { get; set; }
    }
}
