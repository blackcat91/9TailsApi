﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineTails.DataAccess.Models
{
    public class Links
    {
  
    public int Id { get; set; }
    public int? SeriesId { get; set; }
	public int? Episode { get; set; }
	public string Source { get; set; }
	public string Link { get; set; }
    public string Url { get; set; }


    }
}
