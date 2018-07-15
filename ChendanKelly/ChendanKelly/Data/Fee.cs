using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public class Fee
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PartnerOrderId { get; set; }
        public string FeeType { get; set; }
        public double? FeeAmount { get; set; }
        public DateTime? OriginSourceDate { get; set; }
    }
}
