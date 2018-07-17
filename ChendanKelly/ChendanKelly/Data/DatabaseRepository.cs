using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChendanKelly.Data
{
    public interface IDatabaseRepository
    {
        Task InsertDataToOrderTableAsync(List<Order> newOrders, DateTime date);
        Task DeleteDataFromOrderTableAsync(DateTime date);
    }

    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DatabaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteDataFromOrderTableAsync(DateTime date)
        {
            var orders = _dbContext.Orders.Where(p => p.OriginSourceDate == date).ToList();
            foreach (var od in orders)
            {
                _dbContext.Orders.Remove(od);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertDataToOrderTableAsync(List<Order> newOrders, DateTime date)
        {
            foreach(var od in newOrders)
            {
                od.OriginSourceDate = date;
                _dbContext.Orders.Add(od);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
