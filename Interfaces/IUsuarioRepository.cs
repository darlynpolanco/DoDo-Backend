using DoDo.Data.Entities;

namespace DoDo.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorCorreoAsync(string correo);
        Task CrearAsync(Usuario usuario);
    }
}
