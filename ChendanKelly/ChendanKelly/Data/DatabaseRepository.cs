using ChendanKelly.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public interface IDatabaseRepository
    {
        Task<List<File>> GetAllFilesAsync();

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
