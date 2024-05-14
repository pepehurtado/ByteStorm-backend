using System.Text.Json.Serialization;

namespace ByteStorm.Models
{
    public class Operativo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rol { get; set; }

        public virtual List<Mision>? MisionAsignada { get; set;} = new List<Mision>();
    }
}
