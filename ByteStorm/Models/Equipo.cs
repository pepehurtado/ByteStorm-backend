using ByteStorm.Models;

public class Equipo
{
    public int Id { get; set; }

    public enum Tipo
    {
        Software,
        Hardware
    }

    public Tipo TipoEquipo { get; set; }

    public string descripcion { get; set; }

    public enum Estado
    {
        Disponible,
        EnUso
    }

    public Estado EstadoEquipo { get; set; }

    public virtual Mision? perteneceMision { get; set; }
    public int? MisionId { get; set; }
}
