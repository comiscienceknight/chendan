﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public class Baobei
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string BaobeiExternalId { get; set; }
        public int Quantity { get; set; }
        public string BaobeiTitle { get; set; }
        public DateTime? OriginSourceDate { get; set; }
    }
}
