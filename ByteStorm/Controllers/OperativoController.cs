using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStorm.Models;
using Microsoft.AspNetCore.Authorization;
using ByteStorm.Repositorio;
using Microsoft.AspNetCore.SignalR;

namespace ByteStorm.Controllers
{
    
    [Route("api/Operativo")]
    [ApiController]
    public class OperativoController : ControllerBase
    {
        private readonly IRepositorioOperativo _repositorio;
        private readonly IRepositorioMision _repositorioMision;
        private readonly IHubContext<ChatHub> _hubContext; 
        

        public OperativoController(IRepositorioOperativo repositorio, IRepositorioMision repositorioMision, IHubContext<ChatHub> hubContext)
        {
            _repositorio = repositorio;
            _repositorioMision = repositorioMision;
            _hubContext = hubContext;
        }

        // GET: api/Operativo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operativo>>> GetOperativoItems()
        {
            var resp = await _repositorio.GetAsync();
            return resp.ToList();
        }

        // GET: api/Operativo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operativo>> GetOperativo(int id)
        {
            var resp = await _repositorio.GetByIdAsync(id);

            if (resp == null)
            {
                return NotFound();
            }

            return resp;
        }

        // PUT: api/Operativo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperativo(int id, CreateOperativoDTO createOperativoDTO)
        {

            var existingOperativo = await _repositorio.GetByIdAsync(id);

            if (existingOperativo == null)
            {
                return NotFound();
            }

            existingOperativo.Name = createOperativoDTO.Name;
            existingOperativo.Rol = createOperativoDTO.Rol;

            _repositorio.ModificarAsync(existingOperativo);
            _repositorio.Save();
            return NoContent();
        }

        // POST: api/Operativo
        [HttpPost]
        public async Task<ActionResult<OperativoDTO>> PostOperativo(CreateOperativoDTO CreateOperativoDTO)
        {
            var operativo = new Operativo
            {
                Name = CreateOperativoDTO.Name,
                Rol = CreateOperativoDTO.Rol
            };

            _repositorio.AñadirAsync(operativo);
            _repositorio.Save();

            await _hubContext.Clients.All.SendAsync("sendMessage", operativo.Name);

            return CreatedAtAction(nameof(GetOperativo), new { id = operativo.Id }, OperativoToDTO(operativo));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Operativo>> addMisionToOperativo(int id, int idMision)
        {
            var operativo = await _repositorio.GetByIdAsync(id);
            if (operativo == null) {  return NotFound("Operativo not found"); }

            var mision = await _repositorioMision.GetByIdAsync(idMision);
            if (mision == null) { return NotFound("Misión not found"); }
            operativo.MisionAsignada.Add(mision);
            _repositorio.Save();
            return NoContent();
        }

        // DELETE: api/Operativo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperativo(int id)
        {

            var operativo = await _repositorio.GetByIdAsync(id);
            if (operativo == null)
            {
                return NotFound();
            }

            _repositorio.EliminarAsync(id);
            _repositorio.Save();
            return NoContent();
        }


        private static OperativoDTO OperativoToDTO(Operativo operativo) =>
           new OperativoDTO
           {
               Id = operativo.Id,
               Name = operativo.Name,
               Rol = operativo.Rol,
               MisionAsignada = operativo.MisionAsignada
               // Puedes agregar otras propiedades según sea necesario
           };
    }
}
