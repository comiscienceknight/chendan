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

        Task<List<Settle>> GetSettlesAsync(string date);
        Task InsertDataToSettleTableAsync(List<Settle> newSettles, DateTime date,
            string fileName);
        Task DeleteDataFromSettleTableAsync(DateTime date, int fileId);
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
            List<Price> relatedPrices = await GetBaobeiTotalResultsAsync(date, result);

            result.Transactions = new List<TransactionResult>();
            await GetTransactionBaobeiInfosAsync(date, result, relatedPrices);

            // Get all fees of a transaction
            await GetTransactionFeeInfosAsync(date, result);
            
            await GetTransactionSettleInfosAsync(date, result);

            return result;
        }

        private async Task GetTransactionSettleInfosAsync(DateTime date, ResultViewModel result)
        {
            var orderSettlesQuery = from o in _dbContext.Orders
                                 join s in _dbContext.Settles
                                 on (o.OrderId ?? "").Trim() equals (s.OrderId ?? "").Trim()
                                 where o.OriginSourceDate != null && o.OriginSourceDate.Value.Date == date.Date &&
                                    !string.IsNullOrWhiteSpace(o.OrderId)
                                 select new
                                 {
                                     OrderId = o.OrderId,
                                     SettleAmount = s.SettleAmount ?? 0,
                                     SellPrice = s.Amount ?? 0
                                 };
            var orderSettles = await orderSettlesQuery.ToListAsync();
            foreach (var settle in orderSettles)
            {
                var transaction = result.Transactions.FirstOrDefault(p => p.OrderId == settle.OrderId);
                if (transaction != null)
                {
                    transaction.SellPrice = settle.SellPrice.ToString();
                    transaction.Settle = settle.SettleAmount.ToString();
                }
            }
        }

        private async Task GetTransactionFeeInfosAsync(DateTime date, ResultViewModel result)
        {
            var orderFeesQuery = from o in _dbContext.Orders
                                 join f in _dbContext.Fees
                                 on (o.OrderId ?? "").Trim() equals (f.PartnerOrderId ?? "").Trim()
                                 where o.OriginSourceDate != null && o.OriginSourceDate.Value.Date == date.Date &&
                                    !string.IsNullOrWhiteSpace(o.OrderId)
                                 select new
                                 {
                                     OrderId = o.OrderId,
                                     OrderTotalAmout = o.Amount,
                                     FeeType = (!string.IsNullOrWhiteSpace(f.FeeType) && f.FeeType.Contains("Taobaoke")) ? "Taobaoke" : f.FeeType,
                                     FeeAmount = f.FeeAmount
                                 };
            var orderFees = await orderFeesQuery.ToListAsync();
            var orderFeeGroups = orderFees.GroupBy(p => p.OrderId).ToList();
            foreach (var group in orderFeeGroups)
            {
                foreach (var item in group)
                {
                    var transaction = result.Transactions.FirstOrDefault(p => p.OrderId == group.Key);
                    if (transaction == null)
                        transaction = AddNewEmptyTransaction(result, group.Key);
                    string feeType = item.FeeType;
                    if(transaction.FeeResults.Any(p=>p.FeeType == feeType))
                    {
                        transaction.FeeResults.First(p => p.FeeType == feeType).FeeAmount += (item.FeeAmount ?? 0);
                    }
                    else
                    {
                        transaction.FeeResults.Add(new FeeResultPerTransaction
                        {
                            FeeAmount = item.FeeAmount ?? 0,
                            FeeType = feeType
                        });
                    }
                }
            }

            result.FeeTypes = orderFees.Select(p => p.FeeType).Distinct().ToList();
        }

        private async Task GetTransactionBaobeiInfosAsync(DateTime date, ResultViewModel result, List<Price> relatedPrices)
        {
            var ordersQuery = from o in _dbContext.Orders
                              join b in _dbContext.Baobeis
                              on o.OrderId equals b.OrderId
                              where o.OriginSourceDate != null && o.OriginSourceDate.Value.Date == date.Date
                              select new
                              {
                                  OrderId = o.OrderId,
                                  OrderTotalAmout = o.Amount,
                                  BaobeiId = b.BaobeiExternalId,
                                  BaobeiTitle = b.BaobeiTitle,
                                  BaobeiQuantity = b.Quantity
                              };
            var orderBaobeis = await ordersQuery.ToListAsync();
            var orderGroups = orderBaobeis.GroupBy(p => p.OrderId);
            foreach (var group in orderGroups)
            {
                var transaction = new TransactionResult
                {
                    OrderId = group.Key,
                    BaobeiResults = new List<BaobeiResultViewModel>(),
                    FeeResults = new List<FeeResultPerTransaction>()
                };
                result.Transactions.Add(transaction);
                foreach (var item in group)
                {
                    var price = relatedPrices.FirstOrDefault(p => p.BaobeiId == item.BaobeiId);
                    transaction.BaobeiResults.Add(new BaobeiResultViewModel
                    {
                        BaobeiId = item.BaobeiId,
                        BaobeiTitle = item.BaobeiTitle,
                        Quantity = item.BaobeiQuantity,
                        Amount = (price == null ? 0 : ((price.UnitPriceInEuro ?? 0) * item.BaobeiQuantity))
                    });
                }
            }
        }

        private async Task<List<Price>> GetBaobeiTotalResultsAsync(DateTime date, ResultViewModel result)
        {
            var baobeiAndPriceQuery = from b in _dbContext.Baobeis
                                      join p in _dbContext.Prices
                                      on b.BaobeiExternalId equals p.BaobeiId
                                      where b.OriginSourceDate != null && b.OriginSourceDate.Value.Date == date.Date
                                      select new
                                      {
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
            var relatedPrices = _dbContext.Prices.Where(p => p.OriginSourceDate != null && p.OriginSourceDate.Value.Date == date.Date &&
                result.BaobeiTotalResults.Any(pp => pp.BaobeiId == p.BaobeiId)).ToList();
            foreach (var item in result.BaobeiTotalResults)
            {
                var price = relatedPrices.FirstOrDefault(p => p.BaobeiId == item.BaobeiId);
                if (price != null)
                    item.Amount = item.Quantity * (price.UnitPriceInEuro ?? 0);
            }

            return relatedPrices;
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


        #region Price
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


        #region Settle
        public async Task<List<Settle>> GetSettlesAsync(string date)
        {
            var prices = await _dbContext.Settles.Where(p => p.OriginSourceDate != null &&
                p.OriginSourceDate.Value.Date == Convert.ToDateTime(date)).ToListAsync();
            return prices;
        }

        public async Task InsertDataToSettleTableAsync(List<Settle> newSettles, DateTime date, string fileName)
        {
            foreach (var s in newSettles)
            {
                s.OriginSourceDate = date;
                _dbContext.Settles.Add(s);
            }
            _dbContext.Files.Add(new File()
            {
                Date = date,
                FileName = fileName,
                FileType = FileTypeEnum.Settle.ToString()
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDataFromSettleTableAsync(DateTime date, int fileId)
        {
            var settles = _dbContext.Settles.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var bb in settles)
            {
                _dbContext.Settles.Remove(bb);
            }
            var file = _dbContext.Files.FirstOrDefault(p => p.Id == fileId);
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
