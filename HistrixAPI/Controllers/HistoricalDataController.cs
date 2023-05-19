using CsvHelper.Configuration;
using CsvHelper;
using HistrixAPI.Enums;
using HistrixAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HistrixAPI.Repository.Abstract;

namespace HistrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalDataController : Controller
    {
        private readonly IGenericRepository<CryptoPair> _cryptoPairRepository;
        private readonly IGenericRepository<Timeframe> _timeframeRepository;
        private readonly IGenericRepository<CandleEntity> _candleEntityRepository;
        private readonly IBulkRepository<CandleEntity> _candleEntityBulkRepository;

        public HistoricalDataController(
            IGenericRepository<CryptoPair> cryptoPairRepository,
            IGenericRepository<Timeframe> timeframeRepository,
            IGenericRepository<CandleEntity> candleEntityRepository,
            IBulkRepository<CandleEntity> candleEntityBulkRepository)
        {
            _cryptoPairRepository = cryptoPairRepository;
            _timeframeRepository = timeframeRepository;
            _candleEntityRepository = candleEntityRepository;
            _candleEntityBulkRepository = candleEntityBulkRepository;
        }

        [HttpPost("upload-csv/{cryptoPairName}/{timeframeDuration}")]
        public async Task<IActionResult> UploadHistoricalData(string cryptoPairName, TimeframeDuration timeframeDuration, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            try
            {
                var cryptoPair = (await _cryptoPairRepository.GetAsync(filter: (x) => x.CryptoPairName == cryptoPairName)).Single();
                var timeframe = (await _timeframeRepository.GetAsync(filter: (x) => x.TimeframeDuration == timeframeDuration)).Single();
                var records = ProcessCsvFile(file, cryptoPair.Id, timeframe.Id);
                var candleEntityIds = (await _candleEntityRepository.GetAsync(filter: (x) => x.CryptoPairId == cryptoPair.Id && x.TimeframeId == timeframe.Id)).Select(x => x.Id);

                if (candleEntityIds.Any())
                {
                    await _candleEntityBulkRepository.DeleteBatchAsync(candleEntityIds);
                }

                await _candleEntityBulkRepository.InsertBatchAsync(records);

                return Ok(records.Take(10));
            }
            catch (Exception)
            {
                return StatusCode(500, "UploadHistoricalData: An error occurred while uploading historical data.");
            }
        }

        private List<CandleEntity> ProcessCsvFile(IFormFile file, int cryptoPairId, int timeframeId)
        {
            List<CandleEntity> records;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CandleEntityMap>();
                records = csv.GetRecords<CandleEntity>().ToList();

                foreach (var record in records)
                {
                    record.CryptoPairId = cryptoPairId;
                    record.TimeframeId = timeframeId;
                }
            }

            return records;
        }
    }

    public class CandleEntityMap : ClassMap<CandleEntity>
    {
        public CandleEntityMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Open).Name("Open");
            Map(m => m.Close).Name("Close");
            Map(m => m.High).Name("High");
            Map(m => m.Low).Name("Low");
            Map(m => m.Volume).Name("Volume");
        }
    }
}
