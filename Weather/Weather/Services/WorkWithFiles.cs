using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using Weather.Exceptions;
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
            IWorkbook hssfwb;
            try
            {
                hssfwb = WorkbookFactory.Create(stream);
            }
            catch
            {
                throw new ArgumentException("Файл не является excel");
            }

            if (hssfwb.GetSheetAt(0) == null)
            {
                throw new NullReferenceException("Файл пустой");
            }

            foreach (ISheet sheet in hssfwb)
            {
                await SheetProcessingAsync(sheet);
            }
        }

        public Task<List<WeatherInfo>> GetFilteredWeather(int month, int year, int offset, int count)
        {
            var weatherRequest = _context.WeatherInfos.AsQueryable();

            if (month != 0)
            {
                weatherRequest = weatherRequest.Where(w => w.DateOfTaking.Month == month);
            }
            if (year != 0)
            {
                weatherRequest = weatherRequest.Where(w => w.DateOfTaking.Year == year);
            }

            return weatherRequest.OrderBy(w => w.DateOfTaking.Day)
                          .ThenBy(w => w.TimeOfTaking)
                          .Skip(offset)
                          .Take(count)
                          .ToListAsync();
        }

        public Task<int> GetCountOfElements(int month, int year)
        {
            var weatherRequest = _context.WeatherInfos.AsQueryable();

            if(month != 0)
            {
                weatherRequest = weatherRequest.Where(w => w.DateOfTaking.Month == month);
            }
            if(year != 0)
            {
                weatherRequest = weatherRequest.Where(w => w.DateOfTaking.Year == year);
            }
            
            return weatherRequest.CountAsync();
        }


        private async Task SheetProcessingAsync(ISheet sheet)
        {
            if(sheet == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                const string testText = "VV";
                var testCell = sheet.GetRow(2).GetCell(10);
                if (testText != testCell.ToString())
                {
                    throw new Exception();
                }
            }
            catch 
            {
                throw new ArgumentException("Данный файл не содержит в себе необходимую информацию");
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

            if (!_context.ChangeTracker.HasChanges())
            {
                throw new NoOperationsException();
            }

            await _context.SaveChangesAsync();
        }
    }
}