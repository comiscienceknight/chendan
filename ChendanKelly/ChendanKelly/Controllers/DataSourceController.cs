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
        public async Task<ResultViewModel> GetResult()
        {
            return await _dbRepo.GetResultAsync(Convert.ToDateTime("2018-07-18"));
        }

        #region orders
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
        #endregion


        #region Baobei
        [Route("[action]")]
        [HttpGet]
        public async Task<List<Baobei>> GetBaobeis(string date)
        {
            return await _dbRepo.GetBaobeisAsync(date);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task InsertDataToBaobeiTable([FromBody]InsertDataToBaobeiTableViewModel model)
        {
            await _dbRepo.InsertDataToBaobeiTableAsync(model.NewBaobeis, model.Date, model.FileName);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task DeleteDataFromBaobeiTableAsync([FromBody]File model)
        {
            await _dbRepo.DeleteDataFromBaobeiTableAsync(model.Date, model.Id);
        }
        #endregion


        #region Fee
        [Route("[action]")]
        [HttpGet]
        public async Task<List<Fee>> GetFees(string date)
        {
            return await _dbRepo.GetFeesAsync(date);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task InsertDataToFeeTable([FromBody]InsertDataToFeeTableViewModel model)
        {
            await _dbRepo.InsertDataToFeeTableAsync(model.NewFees, model.Date, model.FileName);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task DeleteDataFromFeeTableAsync([FromBody]File model)
        {
            await _dbRepo.DeleteDataFromFeeTableAsync(model.Date, model.Id);
        }
        #endregion


        #region Price
        [Route("[action]")]
        [HttpGet]
        public async Task<List<Price>> GetPrices(string date)
        {
            return await _dbRepo.GetPricesAsync(date);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task InsertDataToPriceTable([FromBody]InsertDataToPriceTableViewModel model)
        {
            await _dbRepo.InsertDataToPriceTableAsync(model.NewPrices, model.Date, model.FileName);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task DeleteDataFromPriceTableAsync([FromBody]File model)
        {
            await _dbRepo.DeleteDataFromPriceTableAsync(model.Date, model.Id);
        }
        #endregion



    }
}
