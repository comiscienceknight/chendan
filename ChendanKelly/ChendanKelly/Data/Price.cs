using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public class Price
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BaobeiId { get; set; }
        public string BaobeiTitle { get; set; }
        public double? UnitPriceInEuro { get; set; }
        public DateTime? OriginSourceDate { get; set; }
    }
}
