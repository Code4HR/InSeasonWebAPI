﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSeasonAPI.Models
{
    class Range
    {
        public Dictionary<string, Location> places { get; set; }
        public NestedSeason season { get; set; }
    }
}
