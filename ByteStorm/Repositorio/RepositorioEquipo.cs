using ByteStorm.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ByteStorm.Repositorio
{
    public class RepositorioEquipo : RepositorioGenerico<Equipo>, IRepositorioEquipo
    {
        public RepositorioEquipo(ByteStormContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Equipo>> GetAsync()
        {
            return await table.Include(p => p.perteneceMision).ToListAsync();
        }

        public async override Task<Equipo> GetByIdAsync(int id)
        {
            return await table.Include(p => p.perteneceMision).Where(o => o.Id == id).FirstOrDefaultAsync();
        }


    }
}
