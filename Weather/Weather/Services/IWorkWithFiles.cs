using Weather.Models;

namespace Weather.Services
{
    public interface IWorkWithFiles
    {
        public Task SaveWeatherInDB(MemoryStream stream);
        public Task<List<WeatherInfo>> GetFilteredWeather(int month, int year, int offset, int count);
        public Task<int> GetCountOfElements(int month, int year);
    }
}
