using ChendanKelly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Models
{
    public class InsertDataToPriceTableViewModel
    {
        public List<Price> NewPrices { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
    }
}
