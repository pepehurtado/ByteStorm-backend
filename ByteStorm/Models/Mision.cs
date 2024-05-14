namespace ByteStorm.Models
{


    public class Mision
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public enum EstadoM
        {
            Planificada,
            Activa,
            Completada
        }
        public EstadoM EstadoMision { get; set; }

        public virtual Operativo? OpAsig { get; set; }
        public int? OpId { get; set; }
        public virtual List<Equipo>? EquipoAsignado { get; set; } = new List<Equipo>();
    }
}
