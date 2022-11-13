namespace Horoscope.DAL.Entities
{
    public class Prediction : BaseEntity<Guid>
    {
        public string Text { get; set; }

        public Guid ZodiakSignId { get; set; }

        public ZodiakSign ZodiakSign { get; set; }
    }
}
