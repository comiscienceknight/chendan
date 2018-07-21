using ChendanKelly.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public interface IDatabaseRepository
    {
        Task<List<File>> GetAllFilesAsync();
        Task<ResultViewModel> GetResultAsync(DateTime date);

        Task<List<Order>> GetOrdersAsync(string date);
        Task InsertDataToOrderTableAsync(List<Order> newOrders, DateTime date,
            string fileName);
        Task DeleteDataFromOrderTableAsync(DateTime date, int fileId);

        Task<List<Baobei>> GetBaobeisAsync(string date);
        Task InsertDataToBaobeiTableAsync(List<Baobei> newOrders, DateTime date,
            string fileName);
        Task DeleteDataFromBaobeiTableAsync(DateTime date, int fileId);


        Task<List<Fee>> GetFeesAsync(string date);
        Task InsertDataToFeeTableAsync(List<Fee> newFees, DateTime date,
            string fileName);
        Task DeleteDataFromFeeTableAsync(DateTime date, int fileId);

        Task<List<Price>> GetPricesAsync(string date);
        Task InsertDataToPriceTableAsync(List<Price> newPrices, DateTime date,
            string fileName);
        Task DeleteDataFromPriceTableAsync(DateTime date, int fileId);
    }

    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DatabaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<File>> GetAllFilesAsync()
        {
            return await _dbContext.Files.ToListAsync();
        }

        public async Task<ResultViewModel> GetResultAsync(DateTime date)
        {
            var result = new ResultViewModel();
            var baobeiAndPriceQuery = from b in _dbContext.Baobeis
                                      join p in _dbContext.Prices
                                      on b.BaobeiExternalId equals p.BaobeiId
                                      where b.OriginSourceDate != null && b.OriginSourceDate.Value.Date == date.Date
                                      select new {
                                          BaobeiId = b.BaobeiExternalId,
                                          Quantity = b.Quantity,
                                          UnitPrice = p.UnitPriceInEuro,
                                          BaobeiTitle = b.BaobeiTitle,
                                          OrderId = b.OrderId
                                      };
            var baobeiAndPriceArray = await baobeiAndPriceQuery.ToListAsync();
            var baobeiAndPriceGroups = baobeiAndPriceArray.GroupBy(p => p.BaobeiId);
            result.BaobeiTotalResults = new List<BaobeiResultViewModel>();
            foreach (var item in baobeiAndPriceGroups)
            {
                result.BaobeiTotalResults.Add(new BaobeiResultViewModel
                {
                    BaobeiId = item.Key,
                    Quantity = item.Sum(p => p.Quantity),
                    BaobeiTitle = item.First().BaobeiTitle,
                    Amount = 0
                });
            }
            var relatedPrices = _dbContext.Prices.Where(p => result.BaobeiTotalResults.Any(pp=>pp.BaobeiId == p.BaobeiId));
            foreach (var item in result.BaobeiTotalResults)
            {
                var price = relatedPrices.FirstOrDefault(p => p.BaobeiId == item.BaobeiId);
                if (price != null)
                    item.Amount = item.Quantity * (price.UnitPriceInEuro ?? 0);
            }

            // Set transaction datas
            var orders = _dbContext.Orders.Where(p => p.OriginSourceDate != null && p.OriginSourceDate.Value.Date == date.Date).ToList();
            result.Transactions = new List<TransactionResult>();
            foreach (var order in orders)
            {
                result.Transactions.Add(new TransactionResult
                {
                    OrderId = order.OrderId,
                    BaobeiResults = new List<BaobeiResultViewModel>(),
                    FeeResults = new List<FeeResultPerTransaction>()
                });
            }
            // Get all baobei records of a transaction
            foreach (var baobei in baobeiAndPriceQuery)
            {
                var transaction = result.Transactions.FirstOrDefault(p => p.OrderId == baobei.OrderId);
                if (transaction == null)
                    transaction = AddNewEmptyTransaction(result, baobei.OrderId);
                var price = relatedPrices.FirstOrDefault(p => p.BaobeiId == baobei.BaobeiId);
                transaction.BaobeiResults.Add(new BaobeiResultViewModel
                {
                    BaobeiId = baobei.BaobeiId,
                    BaobeiTitle = baobei.BaobeiTitle,
                    Quantity = baobei.Quantity,
                    Amount = (price == null ? 0 : ((price.UnitPriceInEuro ?? 0) * baobei.Quantity))
                });
            }
            // Get all fees of a transaction
            var fees = _dbContext.Fees.Where(p => p.OriginSourceDate != null && p.OriginSourceDate.Value.Date == date.Date).ToList();
            for(int i=0;i<fees.Count;i++)
                if (fees[i].FeeType != null && fees[i].FeeType.Contains("Taobaoke"))
                    fees[i].FeeType = "Taobaoke";
            foreach (var fee in fees)
            {
                var transaction = result.Transactions.FirstOrDefault(p => p.OrderId == fee.PartnerOrderId);
                if (transaction == null)
                    transaction = AddNewEmptyTransaction(result, fee.PartnerOrderId);
                transaction.FeeResults.Add(new FeeResultPerTransaction
                {
                    FeeAmount = fee.FeeAmount ?? 0,
                    FeeType = fee.FeeType
                });
            }

            // FeeTypes list
            result.FeeTypes = fees.Select(p => p.FeeType).Distinct().ToList();

            return result;
        }

        private static TransactionResult AddNewEmptyTransaction(ResultViewModel result, string orderId)
        {
            TransactionResult transaction = new TransactionResult
            {
                OrderId = orderId,
                BaobeiResults = new List<BaobeiResultViewModel>(),
                FeeResults = new List<FeeResultPerTransaction>()
            };
            result.Transactions.Add(transaction);
            return transaction;
        }


        #region orders
        public async Task<List<Order>> GetOrdersAsync(string date)
        {
            var orders = await _dbContext.Orders.Where(p => p.OriginSourceDate != null &&
                p.OriginSourceDate.Value.Date == Convert.ToDateTime(date)).ToListAsync();
            return orders;
        }

        public async Task DeleteDataFromOrderTableAsync(DateTime date, int fileId)
        {
            var orders = _dbContext.Orders.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var od in orders)
            {
                _dbContext.Orders.Remove(od);
            }
            var file = _dbContext.Files.FirstOrDefault(p => p.Id == fileId);
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertDataToOrderTableAsync(List<Order> newOrders, DateTime date,
            string fileName)
        {
            foreach(var od in newOrders)
            {
                od.OriginSourceDate = date;
                _dbContext.Orders.Add(od);
            }
            _dbContext.Files.Add(new File()
            {
                Date = date,
                FileName = fileName,
                FileType = FileTypeEnum.Order.ToString()
            });
            await _dbContext.SaveChangesAsync();
        }
        #endregion


        #region baobeis
        public async Task<List<Baobei>> GetBaobeisAsync(string date)
        {
            var baobeis = await _dbContext.Baobeis.Where(p => p.OriginSourceDate != null &&
                p.OriginSourceDate.Value.Date == Convert.ToDateTime(date)).ToListAsync();
            return baobeis;
        }

        public async Task InsertDataToBaobeiTableAsync(List<Baobei> newBaobeis, DateTime date, string fileName)
        {
            foreach (var bb in newBaobeis)
            {
                bb.OriginSourceDate = date;
                _dbContext.Baobeis.Add(bb);
            }
            _dbContext.Files.Add(new File()
            {
                Date = date,
                FileName = fileName,
                FileType = FileTypeEnum.Baobei.ToString()
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDataFromBaobeiTableAsync(DateTime date, int fileId)
        {
            var baobeis = _dbContext.Baobeis.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var bb in baobeis)
            {
                _dbContext.Baobeis.Remove(bb);
            }
            var file = _dbContext.Files.FirstOrDefault(p => p.Id == fileId);
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }
        #endregion


        #region Fee
        public async Task<List<Fee>> GetFeesAsync(string date)
        {
            var fees = await _dbContext.Fees.Where(p => p.OriginSourceDate != null &&
                p.OriginSourceDate.Value.Date == Convert.ToDateTime(date)).ToListAsync();
            return fees;
        }

        public async Task InsertDataToFeeTableAsync(List<Fee> newFees, DateTime date, string fileName)
        {
            foreach (var bb in newFees)
            {
                bb.OriginSourceDate = date;
                _dbContext.Fees.Add(bb);
            }
            _dbContext.Files.Add(new File()
            {
                Date = date,
                FileName = fileName,
                FileType = FileTypeEnum.Fee.ToString()
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDataFromFeeTableAsync(DateTime date, int fileId)
        {
            var fees = _dbContext.Fees.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var fee in fees)
            {
                _dbContext.Fees.Remove(fee);
            }
            var file = _dbContext.Files.FirstOrDefault(p => p.Id == fileId);
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }
        #endregion


        #region
        public async Task<List<Price>> GetPricesAsync(string date)
        {
            try
            {
                var prices = await _dbContext.Prices.Where(p => p.OriginSourceDate != null &&
                    p.OriginSourceDate.Value.Date == Convert.ToDateTime(date)).ToListAsync();
                return prices;
            }
            catch(Exception exp)
            {
                return new List<Price>();
            }
        }

        public async Task InsertDataToPriceTableAsync(List<Price> newPrices, DateTime date, string fileName)
        {
            foreach (var price in newPrices)
            {
                try
                {
                    price.OriginSourceDate = date;
                    _dbContext.Prices.Add(price);
                }
                catch (Exception exp)
                {

                }
            }
            _dbContext.Files.Add(new File()
            {
                Date = date,
                FileName = fileName,
                FileType = FileTypeEnum.Price.ToString()
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDataFromPriceTableAsync(DateTime date, int fileId)
        {
            var prices = _dbContext.Prices.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var price in prices)
            {
                _dbContext.Prices.Remove(price);
            }
            var file = _dbContext.Files.FirstOrDefault(p => p.Id == fileId);
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
