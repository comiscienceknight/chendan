﻿using ChendanKelly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Models
{
    public class InsertDataToBaobeiTableViewModel
    {
        public List<Baobei> NewBaobeis { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
    }
}
