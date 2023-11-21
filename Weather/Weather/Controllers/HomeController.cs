using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Weather.Exceptions;
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
                if(ex is NoOperationsException)
                {
                    return Ok(JsonSerializer.Serialize(new { Text = ex.Message }));
                }
                return Problem(ex.Message);
            }
            return Ok(JsonSerializer.Serialize(new { Text = "Загружено" }));
        }

        [Route("getWeather")]
        [HttpGet]
        public async Task<IActionResult> GetWeather(int month,
                                                    int year,
                                                    int pageNumber,
                                                    int countOFElementsOnPage)
        {
            try
            {
                var entries = await _workWithFiles.GetFilteredWeather(month,
                                                                  year,
                                                                 (pageNumber - 1) * countOFElementsOnPage,
                                                                 countOFElementsOnPage);
                return Ok(entries);
            }
            catch(Exception ex)
            {
                return Problem(JsonSerializer.Serialize(ex.Message));
            }

        }

        [Route("count")]
        [HttpGet]
        public async Task<IActionResult> GetCountWeather(int month, int year)
        {
            try
            {
                var count = await _workWithFiles.GetCountOfElements(month, year);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return Problem(JsonSerializer.Serialize(ex.Message));
            }
            
        }
    }
}
