using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public class Settle
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public double? Amount { get; set; }
        public double? SettleAmount { get; set; }
        public string SettlementTime { get; set; }
        public DateTime? OriginSourceDate { get; set; }
    }
}
