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
            var result = new List<File>();
            for(int i = 0; i < 225; i++)
            {
                result.Add(new Data.File
                {
                    Date = DateTime.UtcNow.AddDays(-i),
                    FileName = DateTime.UtcNow.AddDays(-i).ToString() + ".csv",
                    FileType = "demo",
                    Id = i
                });
            }
            return result;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task InsertDataToOrderTable([FromBody]InsertDataToOrderTableViewModel model)
        {
            await _dbRepo.InsertDataToOrderTableAsync(model.NewOrders, model.Date);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task DeleteDataFromOrderTableAsync([FromBody]DeleteDataFromOrderTableViewModel model)
        {
            await _dbRepo.DeleteDataFromOrderTableAsync(model.Date);
        }
    }
}
