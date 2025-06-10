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

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            var us = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == correo);
            return us;
        }

        public async Task CrearAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == email);
        }

        public async Task<Usuario?> GetByResetTokenAsync(string token)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.ResetPasswordToken == token && u.ResetPasswordTokenExpiration > DateTime.UtcNow);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

    }
}
