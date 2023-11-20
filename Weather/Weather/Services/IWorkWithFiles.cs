using Weather.Models;

namespace Weather.Services
{
    public interface IWorkWithFiles
    {
        public Task SaveWeatherInDB(WeatherContext db, MemoryStream stream);
        public IEnumerable<WeatherInfo> GetFilteredWeather(int month, int year, WeatherContext db, int first, int last);
        public int GetCountOfElements(int month, int year, WeatherContext db);
    }
}
