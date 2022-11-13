using Horoscope.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Horoscope.DAL.Repositories
{
    public class ZodiakSignRepository
    {
        private readonly HoroscopeContext _dbContext;

        public ZodiakSignRepository(HoroscopeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ZodiakSign>> GetAllAsync()
        {
            return await _dbContext.ZodiakSigns.Include(zs => zs.Predictions).ToListAsync();
        }

        public async Task<ZodiakSign> GetByName(string name)
        {
            return await _dbContext.ZodiakSigns.Include(zs => zs.Predictions).FirstAsync(zs => zs.Name == name);
        }
    }
}
