using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Weather.Models
{
    public class WeatherInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateOfTaking { get; set; }

        public TimeSpan TimeOfTaking { get; set; }


        public float Temperature { get; set; }
        [Range(0, 100)]
        public float Humidity { get; set; }

        public float DewPoint { get; set; }

        [Range(700, 800)]
        public int Pressure { get; set; }

        public string WindDirection { get; set; }

        [Range(0, int.MaxValue)]
        public int? WindSpeed { get; set; }

        [Range(0, 100)]
        public int? Cloudiness { get; set; }

        [Range(0, int.MaxValue)]
        public int CloudBase { get; set; }

        [Range(0, int.MaxValue)]
        public int? HorizontalVisibility { get; set; }

        public WeatherCondition? WeatherCondition { get; set; }
    }
}
