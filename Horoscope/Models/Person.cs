using Horoscope.Models.Enums;

namespace Horoscope.Models
{
    public class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public Sex Sex { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
