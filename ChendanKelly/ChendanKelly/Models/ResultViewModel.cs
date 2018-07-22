using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Models
{
    public class ResultViewModel
    {
        public List<BaobeiResultViewModel> BaobeiTotalResults { get; set; }
        public List<TransactionResult> Transactions { get; set; }
        public List<string> FeeTypes { get; set; }
    }
    
    public class TransactionResult
    {
        public string OrderId { get; set; }
        public string SellPrice { get; set; }
        public string Settle { get; set; }
        public List<FeeResultPerTransaction> FeeResults { get; set; }
        public List<BaobeiResultViewModel> BaobeiResults { get; set; }
    }

    public class FeeResultPerTransaction
    {
        public string FeeType { get; set; }
        public double FeeAmount { get; set; }
    }

    public class BaobeiResultViewModel
    {
        public string BaobeiId { get; set; }
        public string BaobeiTitle { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
