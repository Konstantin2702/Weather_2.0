using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Weather.Models;
using Weather.Services;

namespace Weather.Controllers
{
    [ApiController]
    [Route("api/")]
    public class HomeController : ControllerBase
    {
        private readonly IWorkWithFiles _workWithFiles;

        public HomeController(IWorkWithFiles workWithFiles)
        {
            _workWithFiles = workWithFiles;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendFilesName(IFormFile files)
        {
            using var stream = new MemoryStream();
            try
            {
                files.CopyTo(stream);
                await _workWithFiles.SaveWeatherInDB(stream);
            }
            catch (Exception ex)
            {         
                return Problem(JsonSerializer.Serialize(ex.Message));
            }
            return Ok(JsonSerializer.Serialize(new { Text = "Загружено" }));
        }

        [Route("api/weather/GetWeather")]
        [HttpGet]
        public IEnumerable<WeatherInfo> GetWeather(int month, int year, int pageNumber, int countOFElementsOnPage)
        {
            int firstElementToShow = (pageNumber - 1) * countOFElementsOnPage;
            int lastElementToShow = firstElementToShow + countOFElementsOnPage - 1;
            return _workWithFiles.GetFilteredWeather(month, year, _db, firstElementToShow, lastElementToShow);
        }

        [Route("api/weather/GetCountWeather")]
        [HttpGet]
        public int GetCountWeather(int month, int year)
        {
            return _workWithFiles.GetCountOfElements(month, year, _db);
        }
    }
}
