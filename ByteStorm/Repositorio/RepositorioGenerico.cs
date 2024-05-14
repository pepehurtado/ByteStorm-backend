using ByteStorm.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteStorm.Repositorio
{

    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        protected readonly ByteStormContext _context;
        protected DbSet<T> table;

        public RepositorioGenerico(ByteStormContext context)
        {
            _context = context;
            table = _context.Set<T>();

        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            return await table.ToListAsync();
        }
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public void AñadirAsync(T elem)
        {
            table.Add(elem);
        }

        public void EliminarAsync(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }


        public void ModificarAsync(T elem)
        {
            
        }

        public void Save()
        {

           _context.SaveChanges();
        }
    }
}
