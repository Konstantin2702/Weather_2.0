using Microsoft.EntityFrameworkCore;

namespace Weather.Models
{
    public class WeatherContext : DbContext
    {
        public DbSet<WeatherInfo> WeatherInfos => Set<WeatherInfo>();
        public DbSet<WeatherCondition> WeatherConditions => Set<WeatherCondition>();

        public WeatherContext(DbContextOptions<WeatherContext> options)
        : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
