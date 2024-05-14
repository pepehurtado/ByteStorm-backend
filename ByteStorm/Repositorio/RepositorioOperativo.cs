using ByteStorm.Models;
using Microsoft.EntityFrameworkCore;

namespace ByteStorm.Repositorio
{
    public class RepositorioOperativo : RepositorioGenerico<Operativo>, IRepositorioOperativo
    {
        public RepositorioOperativo(ByteStormContext context) : base(context)
        {
        }
        public async override Task<IEnumerable<Operativo>> GetAsync()
        {

            return await table.Include(p => p.MisionAsignada)
                .ToListAsync();
        }

        public async override Task<Operativo> GetByIdAsync(int id)
        {
            return await table.Include(p => p.MisionAsignada)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
