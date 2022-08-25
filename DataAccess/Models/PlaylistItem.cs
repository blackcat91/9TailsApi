using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class PlaylistItem
    {
        public int SeriesId { get; set; }
        public int Episode { get; set; }
        public string? Title { get; set; }
        public string? Poster { get; set; }
        public string? Url { get; set; }

        public string? RoomId { get; set; }
        public string? Status { get; set; } = "Processing";

    }
}
