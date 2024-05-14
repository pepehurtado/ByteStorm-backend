using ByteStorm.Models;
using Microsoft.EntityFrameworkCore;

namespace ByteStorm.Repositorio
{
    public class RepositorioMision : RepositorioGenerico<Mision>, IRepositorioMision
    {
        public RepositorioMision(ByteStormContext context) : base(context)
        {
        }
        public async override Task<IEnumerable<Mision>> GetAsync()
        {
            return await table
            .Include(p => p.OpAsig)
            .Include(q => q.EquipoAsignado)
            .ToListAsync();

        }

        public async override Task<Mision> GetByIdAsync(int id)
        {
            return await table.Include(p => p.EquipoAsignado)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
