namespace Horoscope.DAL.Entities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
