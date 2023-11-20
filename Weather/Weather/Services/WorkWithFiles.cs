using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using Weather.Models;


namespace Weather.Services
{
    public class WorkWithFiles : IWorkWithFiles
    {
        private readonly WeatherContext _context;
        public WorkWithFiles(WeatherContext  context) 
        {
            _context = context;
        }
        public async Task SaveWeatherInDB(MemoryStream stream)
        {
            var hssfwb = WorkbookFactory.Create(stream);

            foreach (ISheet sheet in hssfwb)
            {
                await SheetProcessingAsync(sheet);
            }
        }


        public IEnumerable<WeatherInfo> GetFilteredWeather(int month, int year, WeatherContext db, int first, int last)
        {
            List<WeatherInfo> weatherToSend = new List<WeatherInfo>();
            if (month != 0 && year != 0)
            {
                List<WeatherInfo> weather = db.WeatherInfos
                   .Where(w => w.DateOfTaking.Year == year && w.DateOfTaking.Month == month)
                   .OrderBy(w => w.DateOfTaking.Day)
                   .ThenBy(w => w.TimeOfTaking)
                   .ToList();
                for (int i = 0; i < weather.Count(); i++)
                {
                    if (i >= first && i <= last)
                    {
                        weatherToSend.Add(weather[i]);
                    }
                    else if (i > last)
                        break;

                }
                return weatherToSend;
            }
            else
            {
                return null;
            }
        }

        public int GetCountOfElements(int month, int year, WeatherContext db)
        {
            if (month != 0 && year != 0)
            {
                List<WeatherInfo> weather = db.WeatherInfos
                   .Where(w => w.DateOfTaking.Year == year && w.DateOfTaking.Month == month)
                   .ToList();

                return weather.Count();
            }
            else
                return 0;
        }


        private async Task SheetProcessingAsync(ISheet sheet)
        {
            if(sheet == null)
            {
                throw new NullReferenceException();
            }

            var weatherConditions = await _context.WeatherConditions.ToListAsync();
            var weatherInfos = await _context.WeatherInfos.ToListAsync();  
            var addedConditions = new List<WeatherCondition>();

            for (int row = 4; row <= sheet.LastRowNum; row++)
            {
                var weather = new WeatherInfo();
                var sheetRow = sheet.GetRow(row);

                try
                {
                    if(sheetRow == null) throw new NullReferenceException();

                    weather.DateOfTaking = DateTime.Parse(sheetRow.GetCell(0).ToString());
                    weather.TimeOfTaking = DateTime.Parse(sheetRow.GetCell(1).ToString()).TimeOfDay;

                    if(weatherInfos.Select(w => w.DateOfTaking).Contains(weather.DateOfTaking) &&
                        weatherInfos.Select(w => w.TimeOfTaking).Contains(weather.TimeOfTaking))
                    {
                        continue;
                    }
                    weather.Temperature = float.Parse(sheetRow.GetCell(2).ToString());
                    weather.Humidity = float.Parse(sheetRow.GetCell(3).ToString());
                    weather.DewPoint = float.Parse(sheetRow.GetCell(4).ToString());
                    weather.Pressure = int.Parse(sheetRow.GetCell(5).ToString());

                    weather.WindDirection = sheetRow.GetCell(6).ToString();
                    weather.WindDirection = sheetRow.GetCell(6).ToString();
                    weather.WindSpeed = int.Parse(sheetRow.GetCell(7).ToString());
                    weather.Cloudiness = int.Parse(sheetRow.GetCell(8).ToString());
                    weather.CloudBase = int.Parse(sheetRow.GetCell(9).ToString());
                    weather.HorizontalVisibility = int.Parse(sheetRow.GetCell(10).ToString());


                    var condition = sheetRow.GetCell(11) ?? throw new NullReferenceException();

                    if (!weatherConditions.Select(w => w.Text).Contains(condition.ToString()) &&
                        !addedConditions.Select(w => w.Text).Contains(condition.ToString()))
                    {
                        var newCondition = new WeatherCondition { Text = sheetRow.GetCell(11).ToString() };
                        _context.WeatherConditions.Add(newCondition);
                        addedConditions.Add(newCondition);
                        weather.WeatherCondition = newCondition;
                    }
                    else
                    {
                        weather.WeatherCondition = weatherConditions.FirstOrDefault(w => w.Text == condition.ToString()) ??
                                                   addedConditions.FirstOrDefault(w => w.Text == condition.ToString());
                    }

                    _context.WeatherInfos.Add(weather);
                }
                catch
                {
                    continue;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}