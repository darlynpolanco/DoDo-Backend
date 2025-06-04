using DoDo.Data.Entities;
using DoDo.Data;
using Microsoft.EntityFrameworkCore;
using DoDo.Interfaces;

namespace DoDo.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == correo);
        }

        public async Task CrearAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
