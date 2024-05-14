namespace ByteStorm.Repositorio
{
    public interface IRepositorioGenerico<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T?> GetByIdAsync(int id);
        void ModificarAsync(T elem);
        void AñadirAsync(T elem);
        void EliminarAsync(int id);

        void Save();
    }
}
