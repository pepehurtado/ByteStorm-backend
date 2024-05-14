using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStorm.Models;
using Microsoft.AspNetCore.Authorization;
using ByteStorm.Repositorio;

namespace ByteStorm.Controllers
{
    
    [Route("api/Equipo")]
    [ApiController]
    public class EquipoController : ControllerBase
    {

        private readonly IRepositorioEquipo _repositorio;

        public EquipoController(IRepositorioEquipo repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: api/Equipo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipoItems()
        {
          var resp = await _repositorio.GetAsync();
            return resp.ToList();
        }


        // GET: api/Equipo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            var resp = await _repositorio.GetByIdAsync(id);  

            if (resp == null)
            {
                return NotFound();
            }

            return resp;
        }

        // PUT: api/Equipo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipo(int id, EquipoDTO equipo)
        {
            var existingEquipo = await _repositorio.GetByIdAsync(id);

            if (existingEquipo == null)
            {
                return NotFound();
            }

            // Actualiza las propiedades del Equipo
            existingEquipo.descripcion = equipo.descripcion;
            existingEquipo.TipoEquipo = equipo.TipoEquipoDTO;
            existingEquipo.EstadoEquipo = equipo.EstadoEquipoDTO;
            //existingEquipo.MisionId = equipo.MisionId;
            _repositorio.ModificarAsync(existingEquipo);
            _repositorio.Save();        
            return NoContent();
        }

        // POST: api/Equipo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipo>> PostEquipo(EquipoDTO equipoDTO)
        {
            var equipo = new Equipo
            {
                TipoEquipo = (Equipo.Tipo)equipoDTO.TipoEquipoDTO,
                descripcion = equipoDTO.descripcion,
                EstadoEquipo = (Equipo.Estado)equipoDTO.EstadoEquipoDTO
            };
            _repositorio.AñadirAsync(equipo);
            _repositorio.Save();

            return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, EquipoToDTO(equipo));
        }





        // DELETE: api/Equipo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipo(int id)
        {

            var equipo = await _repositorio.GetByIdAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            _repositorio.EliminarAsync(id);
            _repositorio.Save();
            return NoContent();
        }

        private static Equipo EquipoToDTO(Equipo equipo) =>
            new Equipo
            {
                Id = equipo.Id,
                TipoEquipo = equipo.TipoEquipo,
                descripcion = equipo.descripcion,
                EstadoEquipo = equipo.EstadoEquipo,
                perteneceMision = equipo.perteneceMision
            };
    }
}
