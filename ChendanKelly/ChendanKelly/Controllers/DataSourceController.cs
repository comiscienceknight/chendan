using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChendanKelly.Models;
using ChendanKelly.Data;

namespace ChendanKelly.Controllers
{
    [Route("[controller]")]
    public class DataSourceController : Controller
    {
        public IDatabaseRepository _dbRepo;

        public DataSourceController(IDatabaseRepository dbRepo)
        {
            _dbRepo = dbRepo;
        }
        
        [Route("[action]")]
        [HttpGet]
        public async Task<List<File>> GetAllFiles()
        {
            return await _dbRepo.GetAllFilesAsync();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<List<Order>> GetOrders(string date)
        {
            return await _dbRepo.GetOrdersAsync(date);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task InsertDataToOrderTable([FromBody]InsertDataToOrderTableViewModel model)
        {
            await _dbRepo.InsertDataToOrderTableAsync(model.NewOrders, model.Date, model.FileName);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task DeleteDataFromOrderTableAsync([FromBody]File model)
        {
            await _dbRepo.DeleteDataFromOrderTableAsync(model.Date, model.Id);
        }
    }
}
