namespace Horoscope.DAL.Entities
{
    public class ZodiakSign : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public ICollection<Prediction> Predictions { get; set; }
    }
}
