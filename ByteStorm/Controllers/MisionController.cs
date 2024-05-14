using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStorm.Models;
using Microsoft.Build.Evaluation;
using Microsoft.AspNetCore.Authorization;
using ByteStorm.Repositorio;

namespace ByteStorm.Controllers
{
    
    [Route("api/Mision")]
    [ApiController]
    public class MisionController : ControllerBase
    {
        private readonly IRepositorioMision _repositorio;
        private readonly ByteStormContext _context;

        public MisionController(ByteStormContext context, IRepositorioMision repositorio)
        {

            _context = context;
            _repositorio = repositorio;
        }

        // GET: api/Mision
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mision>>> GetMisionItems()
        {
            var resp = await _repositorio.GetAsync();
            return resp.ToList();
        }

        // GET: api/Mision/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mision>> GetMision(int id)
        {
            var resp = await _repositorio.GetByIdAsync(id);

            if (resp == null)
            {
                return NotFound();
            }

            return resp;
        }


        // PUT: api/Mision/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMision(int id, MisionDTO mision)
        {

            var existingMision = await _repositorio.GetByIdAsync(id);

            if (existingMision == null)
            {
                return NotFound();
            }

            // Actualiza las propiedades de la Mision
            existingMision.Descripcion = mision.Descripcion;
            existingMision.EstadoMision = mision.EstadoMision;
            // existingMision.OpId = mision.OpId;
            _repositorio.ModificarAsync(existingMision);
            _repositorio.Save();
            return NoContent();
        }



        // POST: api/Mision
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mision>> PostMision(MisionDTO MisionDTO)
        {
            var mision = new Mision
            {
                Descripcion = MisionDTO.Descripcion,
                EstadoMision = (Mision.EstadoM)MisionDTO.EstadoMision
            };

            _repositorio.AñadirAsync(mision);
            _repositorio.Save();

            return CreatedAtAction(nameof(GetMision), new { id = mision.Id }, MisionToDTO(mision));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<OperativoDTO>> addEquipoToMision(int id, int idEquipo)
        {

            var mision = await _repositorio.GetByIdAsync(id);
            if (mision == null) { return NotFound("Mision not found"); }
            var equipo = await _context.EquipoItems.Include(p => p.perteneceMision)
                .Where(o => o.Id == idEquipo)
                .FirstOrDefaultAsync();
            //TODO
            if (equipo == null) { return NotFound("Equipo not found"); }

            mision.EquipoAsignado.Add(equipo);
            _repositorio.Save();    
            return NoContent();
        }
       
        // DELETE: api/Mision/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMision(int id)
        {

            var mision = await _repositorio.GetByIdAsync(id);
            if (mision == null)
            {
                return NotFound();
            }

            _repositorio.EliminarAsync(id);
            _repositorio.Save();
            return NoContent();
        }


        private static Mision MisionToDTO(Mision mision) =>
            new Mision
            {
                Id = mision.Id,
                Descripcion = mision.Descripcion,
                EstadoMision = mision.EstadoMision,
                OpAsig = mision.OpAsig
                // Puedes agregar otras propiedades según sea necesario
            };



    }
}
