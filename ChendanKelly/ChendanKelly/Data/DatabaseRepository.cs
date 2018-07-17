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
    }
}
