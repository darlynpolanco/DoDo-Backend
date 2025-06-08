using DoDo.Data.Entities;

namespace DoDo.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorCorreoAsync(string correo);
        Task CrearAsync(Usuario usuario);

        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByResetTokenAsync(string token);
        Task UpdateAsync(Usuario usuario);

    }
}
